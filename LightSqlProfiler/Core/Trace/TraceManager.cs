using LightSqlProfiler.Core.Enums;
using LightSqlProfiler.Gui;
using LightSqlProfiler.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace LightSqlProfiler.Core.Trace
{
    /// <summary>
    /// Class for controlling current trace
    /// </summary>
    public class TraceManager : ObservableObject
    {
        #region Private fields

        private static readonly ILog Log = LogManager.GetLogger(nameof(TraceManager));

        /// <summary>
        /// Currently active trace reader (for reading back the data of DB event)
        /// </summary>
        private TraceReader _traceReader;

        /// <summary>
        /// Current DB connection
        /// </summary>
        private SqlConnection _connection;

        /// <summary>
        /// Cancellation token for aborting data connections
        /// </summary>
        private CancellationTokenSource _readerCancel;

        #endregion Private fields

        #region Events

        /// <summary>
        /// Handler for trace events
        /// </summary>
        /// <param name="ev"></param>
        public delegate void OnEventDelegate(ProfilerEventStatus status, ProfilerEvent ev);

        /// <summary>
        /// Full trace event captured
        /// </summary>
        public event OnEventDelegate OnEvent;

        #endregion Events

        /// <summary>
        /// - Connects to DB
        /// - Registers events
        /// - Opens DB reader
        /// - Starts trace reader BG polling event
        /// </summary>
        public async Task StartTraceSessionAsync(ServerConnection connection, Dictionary<EventClassType, List<EventColumnType>> registeredEvents, CancellationToken cancel)
        {
            Log.Info("Starting trace session");
            try
            {
                _connection = await new DbConnection().GetNewConnectionAsync(connection, cancel);
            }
            catch
            {
                Log.Warn("Error opening connection");
                throw;
            }

            cancel.ThrowIfCancellationRequested();

            try
            {
                _traceReader = new TraceReader(_connection);
                await _traceReader.RegisterTraceAsync(registeredEvents, cancel);
            }
            catch
            {
                Log.Warn("Could not register trace/events. Closing connection.");
                _connection.Dispose();
                _connection = null;
                _traceReader = null;

                throw;
            }

            cancel.ThrowIfCancellationRequested();

            try
            {
                await _traceReader.OpenDbReaderAsync(cancel);
            }
            catch
            {
                Log.Warn("Could not open DB reader");
                await _traceReader.UnregisterTraceAsync(cancel);
                _connection.Dispose();
                _connection = null;
                _traceReader = null;

                throw;
            }

            cancel.ThrowIfCancellationRequested();

            try
            {
                Log.Debug("Starting BG thread");
                _readerCancel = new CancellationTokenSource();

                // start background thread (without "await", to keep it running)
                var progress = new Progress<Tuple<ProfilerEventStatus, ProfilerEvent>>(p => OnEvent?.Invoke(p.Item1, p.Item2));
                var _ = _traceReader.BackgroundPollingThreadAsync(_readerCancel.Token, progress);
            }
            catch (Exception ex)
            {
                Log.Error("Error starting BG worker", ex);
                throw;
            }
        }

        /// <summary>
        /// Stops active trace:
        /// - Cancels BG polling loop
        /// - Closes DB reader
        /// - Unregisters trace
        /// - Closes DB connection
        /// </summary>
        /// <param name="cancel"></param>
        /// <returns></returns>
        public async Task StopTraceSessionAsync(CancellationToken cancel)
        {
            Log.Debug("Stopping trace session");

            // cancel active readers
            _readerCancel.Cancel();

            try
            {
                await _traceReader.CloseDbReaderAsync(cancel);
            }
            catch
            {
                Log.Warn("Cannot close DB reader");
            }

            cancel.ThrowIfCancellationRequested();

            try
            {
                await _traceReader.UnregisterTraceAsync(cancel);
            }
            catch
            {
                Log.Warn("Cannot unregister DB trace");
            }

            Log.Debug("Terminating connection");
            _connection.Close();
            _connection = null;
            _traceReader = null;
            Log.Debug("Trace stopped");
        }
    }
}
