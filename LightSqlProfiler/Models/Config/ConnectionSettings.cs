using LightSqlProfiler.Gui;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace LightSqlProfiler.Models.Config
{
    public class ConnectionSettings : ObservableObject
    {
        /// <summary>
        /// List of all saved servers
        /// </summary>
        public ObservableCollection<ServerConnection> Servers { get; set; } = new ObservableCollection<ServerConnection>();

        /// <summary>
        /// Points to ID of currently selected connection (used for saving)
        /// </summary>
        public Guid? SelectedServerId { get; set; }

        /// <summary>
        /// Currently active/selected connection from the saved list located by ID
        /// </summary>
        [JsonIgnore]
        public ServerConnection CurrentConnection
        {
            get
            {
                return Servers.FirstOrDefault(x => x.Id == SelectedServerId);
            }
            set
            {
                SelectedServerId = value?.Id;
                OnPropertyChanged();
            }
        }
    }
}
