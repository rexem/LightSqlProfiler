using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace LightSqlProfiler.Core
{
    public static class DbTasks
    {
        /// <summary>
        /// Creates a new trace with "sp_trace_create" call
        /// </summary>
        /// <param name="connection"></param>
        /// <returns>Trace ID used to reference and control it later</returns>
        public static async Task<int> CreateTraceAsync(SqlConnection connection, CancellationToken cancel)
        {
            int result = 0;
            SqlCommand cmd = new SqlCommand
            {
                Connection = connection,
                CommandText = "sp_trace_create",
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.Add("@traceid", SqlDbType.Int).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@options", SqlDbType.Int).Value = 1;
            cmd.Parameters.Add("@trace_file", SqlDbType.NVarChar, 245).Value = DBNull.Value;
            cmd.Parameters.Add("@maxfilesize", SqlDbType.BigInt).Value = DBNull.Value;
            cmd.Parameters.Add("@stoptime", SqlDbType.DateTime).Value = DBNull.Value;
            cmd.Parameters.Add("@filecount", SqlDbType.Int).Value = DBNull.Value;
            cmd.Parameters.Add("@result", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;

            await cmd.ExecuteNonQueryAsync(cancel);

            result = (int)cmd.Parameters["@result"].Value;
            result = result != 0 ? -result : (int)cmd.Parameters["@traceid"].Value;
            return result;
        }

        /// <summary>
        /// Change trace status (activate/stop/delete)
        /// Trace needs to be stopped before deleting it from server
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="id"></param>
        /// <param name="status">0 - stops the trace, 1 - starts the trace, 2 - closes and deletes definition from server.</param>
        public static async Task ControlTraceAsync(SqlConnection connection, int id, int status, CancellationToken cancel)
        {
            SqlCommand cmd = new SqlCommand { Connection = connection, CommandText = "sp_trace_setstatus", CommandType = CommandType.StoredProcedure, CommandTimeout = 0 };
            cmd.Parameters.Add("@traceid", SqlDbType.Int).Value = id;
            cmd.Parameters.Add("@status", SqlDbType.Int).Value = status;
            await cmd.ExecuteNonQueryAsync(cancel);
        }

        /// <summary>
        /// Opens trace data reader (via "sp_trace_getdata")
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="traceId"></param>
        /// <returns></returns>
        public static async Task<Tuple<SqlCommand, SqlDataReader>> GetReaderAsync(SqlConnection connection, int traceId, CancellationToken cancel)
        {
            var readerCmd = new SqlCommand { Connection = connection, CommandText = "sp_trace_getdata", CommandType = CommandType.StoredProcedure, CommandTimeout = 0 };
            readerCmd.Parameters.Add("@traceid", SqlDbType.Int).Value = traceId;
            readerCmd.Parameters.Add("@records", SqlDbType.Int).Value = 0;

            var dbReader = await readerCmd.ExecuteReaderAsync(CommandBehavior.SingleResult, cancel);
            return new Tuple<SqlCommand, SqlDataReader>(readerCmd, dbReader);
        }

        /// <summary>
        /// Registers event to be traced by profiler
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="traceId"></param>
        /// <param name="eventId">Event/class code</param>
        /// <param name="columns">Collection of associated columns for the event-class</param>
        public static async Task SetEventAsync(SqlConnection connection, CancellationToken cancel, int traceId, int eventId, params int[] columns)
        {
            SqlCommand cmd = new SqlCommand { Connection = connection, CommandText = "sp_trace_setevent", CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add("@traceid", SqlDbType.Int).Value = traceId;
            cmd.Parameters.Add("@eventid", SqlDbType.Int).Value = eventId;

            // each column is registered separately
            // define template and keep updating the column value while executing the SQL command
            SqlParameter p = cmd.Parameters.Add("@columnid", SqlDbType.Int);
            cmd.Parameters.Add("@on", SqlDbType.Bit).Value = 1;

            // execute multiple commands for each column
            foreach (int i in columns)
            {
                p.Value = i;
                await cmd.ExecuteNonQueryAsync(cancel);
                cancel.ThrowIfCancellationRequested();
            }
        }
    }
}
