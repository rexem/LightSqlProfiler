using LightSqlProfiler.Gui;

namespace LightSqlProfiler.Models.Config
{
    public class EditorSettings : ObservableObject
    {
        private bool _isWrap;

        /// <summary>
        /// Controls text-wrap for the SQL editor (preview)
        /// </summary>
        public bool IsWrap
        {
            get { return _isWrap; }
            set { _isWrap = value; OnPropertyChanged(); }
        }
    }
}
