using LightSqlProfiler.Core.Enums;
using LightSqlProfiler.Gui;
using LightSqlProfiler.Models;
using log4net;

namespace LightSqlProfiler.Core
{
    /// <summary>
    /// Tracks current state of an application
    /// </summary>
    public class AppState : ObservableObject
    {
        private static readonly ILog Log = LogManager.GetLogger(nameof(AppState));

        private AppStatusCodes _status;

        /// <summary>
        /// Gets or sets current application status
        /// </summary>
        public AppStatusCodes Status
        {
            get
            {
                return _status;
            }
            set
            {
                Log.Debug($"Status change to: {value}");
                _status = value;
                OnPropertyChanged();
                //OnPropertyChanged(nameof(IsReady));
            }
        }

        /// <summary>
        /// Indicates if application is being shut down
        /// Typically it means that any processing is no longer needed
        /// </summary>
        public bool IsExiting { get; set; }

        /// <summary>
        /// Currently active server connection
        /// Only available when connection is established
        /// </summary>
        public ServerConnection ActiveConnection { get; set; }

        public bool IsReady => Status == AppStatusCodes.Ready;
    }
}
