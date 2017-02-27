using LightSqlProfiler.Core;
using LightSqlProfiler.ViewModels;
using MahApps.Metro.Controls;

namespace LightSqlProfiler.Views
{
    /// <summary>
    /// Interaction logic for UserSettingsWindow.xaml
    /// </summary>
    public partial class UserSettingsWindow : MetroWindow
    {
        public UserSettingsWindow(UserSettings settings)
        {
            InitializeComponent();

            this.DataContext = new UserSettingsVM(settings, () => this.Close());
        }
    }
}
