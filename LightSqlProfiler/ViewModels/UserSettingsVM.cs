using LightSqlProfiler.Core;
using LightSqlProfiler.Gui;
using System;
using System.Windows.Input;

namespace LightSqlProfiler.ViewModels
{
    internal class UserSettingsVM : ObservableObject
    {
        public UserSettings Settings { get; set; }

        public ICommand CloseCommand { get; set; }

        public UserSettingsVM(UserSettings settings, Action onClose)
        {
            Settings = settings;

            CloseCommand = new DelegateCommand(
                o => true,
                o => onClose()
            );
        }
    }
}
