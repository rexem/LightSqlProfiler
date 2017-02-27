using LightSqlProfiler.Models.Config;
using LightSqlProfiler.ViewModels;
using MahApps.Metro.Controls;
using System;

namespace LightSqlProfiler.Views
{
    /// <summary>
    /// Interaction logic for ConnectWindow.xaml
    /// </summary>
    public partial class ConnectWindow : MetroWindow
    {
        public ConnectWindow(ConnectionSettings settings, Action onConnect)
        {
            InitializeComponent();

            var model = new ConnectVM(onConnect, settings);
            model.CloseAction = this.Close;
            this.DataContext = model;
        }
    }
}
