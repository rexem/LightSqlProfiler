using LightSqlProfiler.Core.Executor;
using LightSqlProfiler.Gui;
using LightSqlProfiler.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LightSqlProfiler.ViewModels
{
    internal class QueryVM : ObservableObject
    {
        private static readonly ILog Log = LogManager.GetLogger(nameof(QueryVM));

        public ExecQuery Exec { get; set; }

        private DataTable _results = new DataTable();

        public DataTable Results
        {
            get { return _results; }
            set { _results = value; OnPropertyChanged(); }
        }

        public ICommand RunCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        public SqlPreviewHandler SqlEditor { get; set; }

        private bool _isRunning;

        public bool IsRunning
        {
            get { return _isRunning; }
            set { _isRunning = value; OnPropertyChanged(); }
        }

        public QueryVM(ServerConnection connection, string dbName)
        {
            Exec = new ExecQuery(connection, dbName);

            RunCommand = new DelegateCommandAsync(
                o => !IsRunning,
                ro =>
                {
                    return RunCmd();
                }
            );

            CancelCommand = new DelegateCommand(
                o => IsRunning,
                ro =>
                {
                    Exec.Cancel();
                    IsRunning = false;
                }
            );
        }

        public async Task RunCmd()
        {
            IsRunning = true;
            List<Dictionary<string, object>> res = null;
            string sql = SqlEditor.GetText();

            // clear existing results
            ShowResults(null);

            // run the query
            try
            {
                res = await Exec.RunSqlAsync(sql);
            }
            catch (Exception ex) when (ex?.Message?.Contains("cancel") == true)
            {
                Log.Debug("SQL execution canceled by user");
            }
            catch (Exception ex)
            {
                Log.Warn("Error executing query", ex);
                MessageBox.Show(ex.Message, "Error executing query", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            ShowResults(res);
            IsRunning = false;
        }

        private void ShowResults(List<Dictionary<string, object>> res)
        {
            var results = new DataTable();
            results.DefaultView.AllowEdit = false;
            results.DefaultView.AllowNew = false;
            results.DefaultView.AllowDelete = false;

            // if results available - populate the table
            if (res != null && res.Count > 0)
            {
                var columns = res.First().Keys;
                foreach (var col in columns)
                    results.Columns.Add(col);

                foreach (var row in res)
                {
                    DataRow r = results.NewRow();
                    foreach (var c in row)
                    {
                        r[c.Key] = c.Value;
                    }
                    results.Rows.Add(r);
                }
            }

            Results = results;
            OnPropertyChanged(nameof(Results));
        }
    }
}
