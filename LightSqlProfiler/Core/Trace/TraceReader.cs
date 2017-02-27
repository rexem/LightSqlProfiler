using LightSqlProfiler.Core.Enums;
using LightSqlProfiler.Core.Trace.Definitions;
using LightSqlProfiler.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LightSqlProfiler.Core.Trace
{
    class TraceReader
    {
        private static readonly ILog Log = LogManager.GetLogger(nameof(TraceReader));
        private static readonly ILog EventLog = LogManager.GetLogger(nameof(TraceReader) + ".Event");

        /// <summary>
        /// Id of the current trace as registered in the database
        /// NULL - trace haven't been registered yet
        /// </summary>
        private int? _traceId;
        
        /// <summary>
        /// Currently open DB connection
        /// </summary>
        private SqlConnection _connection;

        /// <summary>
        /// Currently open DB reader's command (allows canceling, since reader itself might be locked)
        /// </summary>
        private SqlCommand _dbCommand;

        /// <summary>
        /// DB reader awaiting trace events
        /// </summary>
        private SqlDataReader _dbReader;

        /// <summary>
        /// Event that is currently being filled/processed.
        /// Each event returns data for a single column. We trigger the final event once all the data is captured, otherwise keep storing it here
        /// </summary>
        private ProfilerEvent _buffer;

        /// <summary>
        /// Stores last "full" event. When even flushes, buffer is transferred to this property.
        /// </summary>
        private ProfilerEvent _lastEvent;

        /// <summary>
        /// Prepares trace reader for upcoming events
        /// Database connection should be open
        /// </summary>
        /// <param name="connection"></param>
        public TraceReader(SqlConnection connection)
        {
            this._connection = connection;
        }

        /// <summary>
        /// Registers trace with the database, opens command and DB data readers.
        /// Setups events/columns to be captured
        /// </summary>
        public async Task RegisterTraceAsync(Dictionary<EventClassType, List<EventColumnType>> registeredEvents, CancellationToken cancel)
        {
            Log.Debug("Creating new trace");
            _traceId = await DbTasks.CreateTraceAsync(_connection, cancel);
            Log.Debug($"Trace ID: {_traceId}");

            // prepare new buffer for receiving data
            _buffer = new ProfilerEvent(EventClassType.Unknown);

            // register each event-class
            foreach (var evClass in registeredEvents.Keys)
            {
                cancel.ThrowIfCancellationRequested();

                // convert all columns for this event-class into a flat array for registration
                var columns = registeredEvents[evClass].Select(x => (int)x).ToArray();

                try
                {
                    Log.Debug($"Registering event ID: {evClass} with {columns.Count()} columns");
                    await DbTasks.SetEventAsync(_connection, cancel, _traceId.Value, (int)evClass, columns);
                }
                catch
                {
                    Log.Warn($"Event registration failed. ID: {evClass}");
                    throw;
                }
            }

            try
            {
                await DbTasks.ControlTraceAsync(_connection, _traceId.Value, 1 /* start */, cancel);
            }
            catch
            {
                Log.Warn("Cannot start registered trace");
                throw;
            }
        }

        /// <summary>
        /// Close and delete the trace (needs to be closed before deleting)
        /// NOP if trace wasn't registered
        /// </summary>
        /// <param name="cancel"></param>
        /// <returns></returns>
        public async Task UnregisterTraceAsync(CancellationToken cancel)
        {
            if (_traceId == null)
            {
                Log.Debug("Trace wasn't registered. Nothing to stop.");
                return;
            }

            try
            {
                Log.Debug("Stopping trace with sp_trace_setstatus, status = 0");
                await DbTasks.ControlTraceAsync(_connection, _traceId.Value, 0 /* stop */, cancel);
                
            }
            catch (Exception ex)
            {
                Log.Warn($"Cannot stop trace. TraceId: {_traceId}", ex);
                throw;
            }

            try
            {
                Log.Debug("Deleting trace with sp_trace_setstatus, status = 2");
                await DbTasks.ControlTraceAsync(_connection, _traceId.Value, 2 /* close and delete */, cancel);
            }
            catch
            {
                Log.Warn($"Cannot delete trace. TraceId: {_traceId}");
                throw;
            }
        }

        public async Task OpenDbReaderAsync(CancellationToken cancel)
        {
            Log.Debug($"Opening DB reader");

            // setup single DB reader and DB command
            var r = await DbTasks.GetReaderAsync(_connection, _traceId.Value, cancel);
            _dbCommand = r.Item1;
            _dbReader = r.Item2;
        }

        public async Task CloseDbReaderAsync(CancellationToken cancel)
        {
            Log.Debug($"Closing DB reader");

            if (_dbReader != null && !_dbReader.IsClosed)
            {
                await Task.Run(() =>
                {
                    try
                    {
                        _dbCommand?.Cancel();
                        cancel.ThrowIfCancellationRequested();
                        _dbReader.Close();
                        _dbCommand.Dispose();
                    }
                    catch (Exception ex)
                    {
                        Log.Warn("Cannot close DB reader cleanly", ex);
                    }
                    finally
                    {
                        _dbCommand = null;
                        _dbReader = null;
                    }
                });
            }
        }

        /// <summary>
        /// Method running in the BackgroundWorker context, polling for events
        /// </summary>
        public async Task BackgroundPollingThreadAsync(CancellationToken cancel, IProgress<Tuple<ProfilerEventStatus, ProfilerEvent>> progress)
        {
            Log.Debug("Starting BG polling thread");

            // repeat until canceled or database connection lost
            while (_dbReader != null && !_dbReader.IsClosed)
            {
                try
                {
                    bool res = await _dbReader.ReadAsync(cancel);
                    if (_dbReader.IsClosed)
                        break;

                    // nothing to read, continue
                    if (!res)
                        continue;

                    cancel.ThrowIfCancellationRequested();

                    // we read something - process the event
                    var status = ProcessEvent();

                    progress.Report(new Tuple<ProfilerEventStatus, ProfilerEvent>(status, _lastEvent));
                }

                // when cancellation happens, we might close DB before closing readers - this is normal
                catch (Exception ex) when (ex.Message?.Contains("connection is closed") == true && cancel.IsCancellationRequested)
                {
                    Log.Debug("Connection is closed, skipping live DB readers (breaking loop)");
                    break;
                }
                catch (Exception ex)
                {
                    Log.Error($"Got error in DB readers", ex);
                    throw;
                }
            }

            Log.Error("BG polling finished");
        }

        /// <summary>
        /// Process DB reader data
        /// </summary>
        /// <returns>
        /// Status of the processed event:
        /// - "updating" - (still building full event);
        /// - "new-event" meaning all data was received and the event is fully read;
        /// </returns>
        private ProfilerEventStatus ProcessEvent()
        {
            // event codes:

            // 65534 Trace start
            // 65528 First file in trace sequence, seems to appear after all the existing connections events. A bit like this is the start of the live events.
            // 65527 Trace Rollover (i.e.a new file has been started
            // 65533 Trace Stop
            // 65526 start/flush

            // column ID and event ID are always present, read them
            int columnId = Convert.ToInt32(_dbReader[0]);
            
            var buffer = new byte[2];
            _dbReader.GetBytes(2, 0, buffer, 0, 2);
            int eventClassId = (buffer[0]) | (buffer[1] << 8);

            EventLog.Debug($"ProcessEvent: {eventClassId}/{columnId}");

            // column of 65526, indicates a start/flush event, meaning:
            // new trace event is about to happen, thus the current event won't retrieve more data and we can return it as processed
            if (columnId == 65526)
            {
                EventLog.Debug("FLUSH");

                // even is complete
                FlushEvent();

                _buffer = new ProfilerEvent((EventClassType)eventClassId);
                return ProfilerEventStatus.NewEvent;
            }

            // continuation of the same trace event
            
            // find a column for this event, and set/read the value
            var columnDefinition = EventDefinition.Instance.Columns.FirstOrDefault(x => (int)x.ColumnType == columnId);
            if (columnDefinition != null)
            {
                _buffer.SetColumnValue(columnDefinition.ColumnType, columnDefinition.Process(_dbReader));
            }
            else
            {
                EventLog.Debug($"Column was not found; ColumnId: {columnId}");
            }
            
            return ProfilerEventStatus.Update;
        }

        private void FlushEvent()
        {
            _lastEvent = _buffer;

            if (_lastEvent.IsInternal)
                EventLog.Debug("internal");
        }
    }
}
