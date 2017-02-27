using LightSqlProfiler.Core;
using LightSqlProfiler.Gui;
using Newtonsoft.Json;
using System;

namespace LightSqlProfiler.Models
{
    /// <summary>
    /// Represents data for a single server
    /// </summary>
    public class ServerConnection : ObservableObject, ICloneable
    {
        public Guid? Id { get; set; }

        private string _name;
        public string Name { get { return _name; } set { _name = value; OnPropertyChanged(); } }

        private string _hostname;
        public string Hostname { get { return _hostname; } set { _hostname = value; OnPropertyChanged(); } }

        private string _username;
        public string Username { get { return _username; } set { _username = value; OnPropertyChanged(); } }

        public string _password;
        public string Password { get { return _password; } set { _password = value; OnPropertyChanged(); } }

        private bool _rememberPassword;
        public bool RememberPassword { get { return _rememberPassword; } set { _rememberPassword = value; OnPropertyChanged(); } }

        [JsonIgnore]
        public string RawPassword
        {
            get { return Common.DecodePassword(Password); }
            set
            {
                Password = Common.EncodePassword(value);
                OnPropertyChanged();
            }
        }

        public ServerConnection()
        {
            Id = Guid.NewGuid();
        }

        public ServerConnection(string name) : this()
        {
            Name = name;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
