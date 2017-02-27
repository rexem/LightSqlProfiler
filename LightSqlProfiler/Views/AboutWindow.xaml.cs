using LightSqlProfiler.Properties;
using MahApps.Metro.Controls;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows;

namespace LightSqlProfiler.Views
{
    /// <summary>
    /// Interaction logic for AboutWindow.xaml
    /// </summary>
    public partial class AboutWindow : MetroWindow
    {
        private Version _version = Assembly.GetExecutingAssembly().GetName().Version;

        public string HomepageUrl => Settings.Default.HomepageUrl;

        public string VersionString => $"v{_version.Major}.{_version.Minor}";

        public AboutWindow()
        {
            InitializeComponent();

            this.DataContext = this;
        }

        private void Link_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
