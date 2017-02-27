using LightSqlProfiler.Gui;
using LightSqlProfiler.Models;
using LightSqlProfiler.Models.Config;
using System;
using System.Linq;
using System.Windows.Input;

namespace LightSqlProfiler.ViewModels
{
    internal class ConnectVM : ObservableObject
    {
        /// <summary>
        /// Callback when connection is invoked
        /// </summary>
        private Action _onConnect;

        /// <summary>
        /// Link to global user connection settings
        /// </summary>
        public ConnectionSettings Connections { get; set; }

        /// <summary>
        /// Action for closing the window (typically set in the window to exit the screen)
        /// </summary>
        public Action CloseAction { get; set; }

        #region Commands

        public ICommand ConnectCommand { get; }
        public ICommand NewConnectionCommand { get; }
        public ICommand DeleteConnectionCommand { get; }

        #endregion Commands

        public ConnectVM(Action onConnect, ConnectionSettings settings)
        {
            _onConnect = onConnect;
            Connections = settings;

            ConnectCommand = new DelegateCommand(
                o => Connections?.CurrentConnection?.Hostname?.Length > 0,
                o =>
                {
                    CloseAction();
                    _onConnect.Invoke();
                }
            );

            NewConnectionCommand = new DelegateCommand(
                o => true,
                o =>
                {
                    var item = new ServerConnection("Unnamed");
                    Connections.Servers.Add(item);
                    Connections.CurrentConnection = item;
                }
            );

            DeleteConnectionCommand = new DelegateCommand(
                o => Connections?.CurrentConnection != null,
                o =>
                {
                    Connections.Servers.Remove(Connections.CurrentConnection);
                    Connections.CurrentConnection = Connections.Servers.FirstOrDefault();
                }
            );
        }
    }
}
