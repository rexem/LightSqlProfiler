using LightSqlProfiler.Core.Enums;
using LightSqlProfiler.Gui;

namespace LightSqlProfiler.Models
{
    /// <summary>
    /// Controls column appearance in trace results box
    /// </summary>
    public class GuiGridColumn : ObservableObject
    {
        public EventColumnType ColumnType { get; set; }

        public int Width { get; set; } = 100;

        /// <summary>
        /// Name of bound property (this name will be used when retrieving column value in GUI from ProfilerEvent row)
        /// </summary>
        public string BindingName => ColumnType.ToString();

        /// <summary>
        /// User visible name of the column
        /// </summary>
        public string DisplayName => ColumnType.ToString();

        /// <summary>
        /// Currently active filter for the column
        /// </summary>
        public string Filter
        {
            get { return _filter; }
            set { _filter = value; OnPropertyChanged(); }
        }

        private string _filter;

        public GuiGridColumn()
        {
        }

        public GuiGridColumn(EventColumnType evType)
        {
            ColumnType = evType;
        }
    }
}
