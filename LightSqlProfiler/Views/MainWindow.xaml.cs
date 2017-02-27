using LightSqlProfiler.Core;
using LightSqlProfiler.Gui;
using LightSqlProfiler.ViewModels;
using MahApps.Metro.Controls;
using System.Linq;
using System.Windows.Data;

namespace LightSqlProfiler
{
    public partial class MainWindow : MetroWindow
    {
        /// <summary>
        /// View model
        /// </summary>
        private MainVM _model = new MainVM();

        public MainWindow()
        {
            InitializeComponent();

            // load settings
            var settings = UserSettings.LoadSettings();
            _model.Settings = settings;

            // prepare results view
            _model.EventGrid = new EventViewHandler(ResultGridBox, settings, this.Resources["TextConverter"] as IValueConverter);
            _model.EventGrid.BuildColumns(settings.Columns, _model.Events);
            _model.EventGrid.ApplyFilter(_model.Events);

            // prepare SQL code preview control
            _model.SqlPreview = new SqlPreviewHandler(SqlPreviewBox, _model.Settings.Editor);
            _model.SqlPreview.Setup();

            // events
            this.Closing += (ss, ee) => _model.OnExit();

            // auto scroll to bottom
            _model.Events.CollectionChanged += (ss, ee) =>
            {
                if (settings.App.AutoScroll)
                    ResultsContainer.ScrollIntoView(_model.Events.LastOrDefault());
            };

            this.DataContext = _model;
        }
    }
}
