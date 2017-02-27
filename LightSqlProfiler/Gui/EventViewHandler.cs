using LightSqlProfiler.Core;
using LightSqlProfiler.Models;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;

namespace LightSqlProfiler.Gui
{
    /// <summary>
    /// Handles main event-results-view
    /// Includes result filtering, dynamic column showing and tracking resizing
    /// </summary>
    public class EventViewHandler
    {
        private GridView _control;
        private UserSettings _settings;
        private IValueConverter _converter;

        public EventViewHandler(GridView control, UserSettings settings, IValueConverter columnValueConverter)
        {
            _control = control;
            _settings = settings;
            _converter = columnValueConverter;
        }

        /// <summary>
        /// Prepare the GridView control filter (columns should be already added):
        /// - apply filter
        /// - track column changes (recording them to settings file)
        /// </summary>
        /// <param name="events"></param>
        public void ApplyFilter(IEnumerable<ProfilerEvent> events)
        {
            // column filters
            var view = (CollectionViewSource.GetDefaultView(events) as CollectionView).Filter =
                (item) =>
                {
                    var row = item as ProfilerEvent;

                    // filter each column
                    foreach (var col in _settings.Columns)
                        if (!row.PassesFilter(col.ColumnType, col.Filter))
                            return false;
                    return true;
                };

            // save shown columns to user settings
            _control.Columns.CollectionChanged += (ss, ee) => SyncColumns();
        }

        /// <summary>
        /// Create and map columns for the results view
        /// </summary>
        public void BuildColumns(IEnumerable<GuiGridColumn> columns, IEnumerable<ProfilerEvent> events)
        {
            _control.Columns.Clear();
            foreach (var column in columns)
            {
                var col = new GridViewColumn();
                col.Header = column;

                // register two-way binding for column width, so we can store them in settings
                BindingOperations.SetBinding(col, GridViewColumn.WidthProperty,
                    new Binding(nameof(col.Width))
                    {
                        Source = column,
                        UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                        Mode = BindingMode.TwoWay
                    });

                // binding for column content,
                // effectively Model.Events[..BindingName..] needs to show in the cell
                col.DisplayMemberBinding = new Binding(column.BindingName)
                {
                    Converter = _converter
                };

                // when filter updated, refresh view
                column.PropertyChanged += (ss, ee) => ((CollectionView)CollectionViewSource.GetDefaultView(events)).Refresh();

                _control.Columns.Add(col);
            }
        }

        /// <summary>
        /// Save current column layout to user settings
        /// After columns are altered, we want to save their location
        /// </summary>
        private void SyncColumns()
        {
            _settings.Columns.Clear();
            foreach (var col in _control.Columns.Select(x => x.Header as GuiGridColumn))
                _settings.Columns.Add(col);
        }
    }
}
