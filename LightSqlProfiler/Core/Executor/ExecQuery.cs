using Dapper;
using LightSqlProfiler.Gui;
using LightSqlProfiler.Models;
using log4net;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LightSqlProfiler.Core.Executor
{
    public class ExecQuery : ObservableObject
    {
        private static readonly ILog Log = LogManager.GetLogger(nameof(ExecQuery));

        /// <summary>
        /// Connection settings for the server
        /// Each query will run on its own connection using these settings
        /// </summary>
        private ServerConnection _connection;

        /// <summary>
        /// Active database (if any) for which we set "initial catalog" on connection start
        /// </summary>
        public string DbName
        {
            get { return _dbName; }
            set { _dbName = value; OnPropertyChanged(); }
        }
        private string _dbName;

        /// <summary>
        /// Query execution cancellation token
        /// </summary>
        private CancellationTokenSource _cancelSource;

        /// <summary>
        /// Updates settings
        /// </summary>
        public ExecQuery(ServerConnection connection, string dbName)
        {
            _connection = connection;
            DbName = dbName;
        }

        public void Cancel()
        {
            _cancelSource.Cancel();
        }

        public async Task ExecSqlAsync(string sql)
        {
            _cancelSource = new CancellationTokenSource();
            using (var con = await new DbConnection().GetNewConnectionAsync(_connection, _cancelSource.Token, _dbName))
            {
                Log.Info("Executing query");
                await con.ExecuteAsync(new CommandDefinition(sql, commandTimeout: 3600, cancellationToken: _cancelSource.Token));
            }
        }

        public async Task<List<Dictionary<string, object>>> RunSqlAsync(string sql)
        {
            _cancelSource = new CancellationTokenSource();
            var result = new List<Dictionary<string, object>>();

            // todo: limit result set
            // todo: put timeout to settings

            using (var con = await new DbConnection().GetNewConnectionAsync(_connection, _cancelSource.Token, _dbName))
            {
                Log.Info("Executing query");
                var rows = await con.QueryAsync(new CommandDefinition(sql, commandTimeout: 3600, cancellationToken: _cancelSource.Token));
                Log.Info("Executed query");

                foreach (IDictionary<string, object> row in rows)
                {
                    // make sure each column has unique name
                    // on duplicates: add "(N)" to the end
                    // on empty: replace with "no-name"
                    Dictionary<string, object> res = new Dictionary<string, object>();
                    foreach (var item in row)
                    {
                        _cancelSource.Token.ThrowIfCancellationRequested();

                        int copyNo = 2;
                        string originalKey = string.IsNullOrWhiteSpace(item.Key) ? "no-name" : item.Key.Trim();
                        string key = originalKey;

                        while (res.ContainsKey(key))
                        {
                            key = $"{originalKey} ({copyNo})";
                            copyNo++;
                        }

                        res.Add(key, item.Value);
                    }

                    Dictionary<string, object> d = res.ToDictionary(
                        r => r.Key,
                        r => r.Value ?? "(null)"
                    );
                    result.Add(d);
                }
            }

            return result;
        }
    }
}
