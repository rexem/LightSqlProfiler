using LightSqlProfiler.Models;
using log4net;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace LightSqlProfiler.Core
{
    class DbConnection
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(DbConnection).Name);

        /// <summary>
        /// Opens new DB connection
        /// </summary>
        /// <returns></returns>
        public async Task<SqlConnection> GetNewConnectionAsync(ServerConnection connection, CancellationToken cancelToken, string dbName = null)
        {
            Log.Debug($"Connecting to DB server: {connection.Hostname}");
            string connectionString = $"Server={connection.Hostname};User Id={connection.Username};Password={connection.RawPassword};Application Name=Light SQL Profiler;";

            // set initial DB if requested
            if (!string.IsNullOrEmpty(dbName))
                connectionString += $"Initial catalog={dbName};";

            var con = new SqlConnection(connectionString);
            await con.OpenAsync(cancelToken);

            Log.Debug($"Connected to DB: {connection.Hostname}");
            return con;
        }
    }
}
