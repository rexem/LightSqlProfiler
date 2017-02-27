using LightSqlProfiler.Gui;
using LightSqlProfiler.Models;
using LightSqlProfiler.ViewModels;
using MahApps.Metro.Controls;

namespace LightSqlProfiler.Views
{
    /// <summary>
    /// Interaction logic for QueryWindow.xaml
    /// </summary>
    public partial class QueryWindow : MetroWindow
    {
        public QueryWindow(ServerConnection connection, string dbName, string sqlCode)
        {
            InitializeComponent();

            var model = new QueryVM(connection, dbName);

            // setup editor
            model.SqlEditor = new SqlPreviewHandler(SqlEditor, new Models.Config.EditorSettings() { IsWrap = false });
            model.SqlEditor.Setup();
            model.SqlEditor.SetText(sqlCode);

            this.DataContext = model;
        }
    }
}
