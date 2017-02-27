using LightSqlProfiler.Gui;
using Newtonsoft.Json;
using System.Windows;

namespace LightSqlProfiler.Models.Config
{
    public class AppSettings : ObservableObject
    {
        #region Window Location

        public double? WindowWidth { get; set; } = 1000;
        public double? WindowHeight { get; set; } = 500;
        public double? WindowLeft { get; set; }
        public double? WindowTop { get; set; }
        public bool? IsMaximized { get; set; }

        /// <summary>
        /// Note: We don't want to save minimized state, thus only save IsMaximized value
        /// </summary>
        [JsonIgnore]
        public WindowState WindowState
        {
            get { return IsMaximized == true ? WindowState.Maximized : WindowState.Normal; }
            set { IsMaximized = value == WindowState.Maximized; }
        }

        #endregion Window Location

        /// <summary>
        /// Indicates if results should automatically scroll to the end (into view) when new row is added
        /// </summary>
        public bool AutoScroll
        {
            get { return _autoScroll; }
            set { _autoScroll = value; OnPropertyChanged(); }
        }

        private bool _autoScroll = true;

        /// <summary>
        /// If set, custom trace row is added when trace starts
        /// </summary>
        public bool AddTraceStartEvent { get; set; } = true;

        /// <summary>
        /// If set, custom trace row is added when trace stops
        /// </summary>
        public bool AddTraceStopEvent { get; set; } = true;
    }
}
