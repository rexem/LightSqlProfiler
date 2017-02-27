using LightSqlProfiler.Core.Enums;
using LightSqlProfiler.Models;
using LightSqlProfiler.Models.Config;
using LightSqlProfiler.Properties;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LightSqlProfiler.Core
{
    /// <summary>
    /// User/application settings
    /// Serializable entity which is directly stored to configuration file, including children properties
    /// </summary>
    [Serializable]
    public class UserSettings
    {
        [JsonIgnore]
        private static readonly ILog Log = LogManager.GetLogger(typeof(UserSettings).Name);

        /// <summary>
        /// GUI columns shown in result list as well for trace registration
        /// </summary>
        public List<GuiGridColumn> Columns { get; set; }

        /// <summary>
        /// Registered events for trace listening
        /// </summary>
        public List<EventClassType> Events { get; set; }

        /// <summary>
        /// Saved connections to DB
        /// </summary>
        public ConnectionSettings Connections { get; set; }

        /// <summary>
        /// SQL text editor settings
        /// </summary>
        public EditorSettings Editor { get; set; }

        /// <summary>
        /// General app appearance settings
        /// </summary>
        public AppSettings App { get; set; }

        #region Save/Load

        /// <summary>
        /// Prepares config file for saving
        /// Strips out any unwanted data
        /// </summary>
        private void PrepareSettingsSave()
        {
            // do not save passwords which we don't want to remember
            foreach (var item in Connections.Servers.Where(x => !x.RememberPassword))
                item.Password = null;
        }

        /// <summary>
        /// Saves user configuration to a file
        /// </summary>
        public void SaveSettings()
        {
            var path = Common.GetAppFilePath(Settings.Default.ConfigFile);
            Log.Debug($"Saving settings: {path}");

            PrepareSettingsSave();

            // check what is currently saved, to avoid overwriting
            // read existing file, serialize current settings and check for differences
            string existing = string.Empty;
            if (File.Exists(path))
                existing = File.ReadAllText(path);

            var serialized = JsonConvert.SerializeObject(this, Formatting.Indented);

            if (existing != serialized)
                File.WriteAllText(path, serialized);
        }

        /// <summary>
        /// Loads configuration from a config file
        /// </summary>
        /// <returns></returns>
        public static UserSettings LoadSettings()
        {
            var path = Common.GetAppFilePath(Settings.Default.ConfigFile);
            Log.Debug($"Loading settings: {path}");

            // try loading config file
            UserSettings settings = null;
            try
            {
                // might fail if configuration file is not created yet
                settings = JsonConvert.DeserializeObject<UserSettings>(File.ReadAllText(path));
            }
            catch (Exception ex)
            {
                Log.Warn("Cannot read configuration file", ex);
            }

            // validate and/or reset settings
            settings = settings ?? new UserSettings();
            settings.ValidateSettings();
            return settings;
        }

        /// <summary>
        /// Validate current user settings, making sure sane defaults are used
        /// </summary>
        public void ValidateSettings()
        {
            Columns = Columns ?? GetDefaultColumns();
            Connections = Connections ?? new ConnectionSettings();
            Events = Events ?? GetDefaultEvents();
            Editor = Editor ?? new EditorSettings();
            App = App ?? new AppSettings();
        }

        #endregion Save/Load

        #region Defaults
        public List<GuiGridColumn> GetDefaultColumns()
        {
            var items = new List<GuiGridColumn>();
            items.Add(new GuiGridColumn(EventColumnType.EventClass));
            items.Add(new GuiGridColumn(EventColumnType.TextData));
            items.Add(new GuiGridColumn(EventColumnType.ApplicationName));
            items.Add(new GuiGridColumn(EventColumnType.NTUserName));
            items.Add(new GuiGridColumn(EventColumnType.LoginName));
            items.Add(new GuiGridColumn(EventColumnType.CPU));
            items.Add(new GuiGridColumn(EventColumnType.Reads));
            items.Add(new GuiGridColumn(EventColumnType.Writes));
            items.Add(new GuiGridColumn(EventColumnType.Duration));
            items.Add(new GuiGridColumn(EventColumnType.ClientProcessID));
            items.Add(new GuiGridColumn(EventColumnType.SPID));
            items.Add(new GuiGridColumn(EventColumnType.StartTime));
            items.Add(new GuiGridColumn(EventColumnType.EndTime));
            return items;
        }

        public List<EventClassType> GetDefaultEvents()
        {
            return new List<EventClassType>()
            {
                EventClassType.RPCCompleted,
                EventClassType.SQLBatchStarting,
                EventClassType.SQLBatchCompleted
            };
        }
        #endregion
    }
}
