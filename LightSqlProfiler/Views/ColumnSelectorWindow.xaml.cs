using LightSqlProfiler.Core;
using LightSqlProfiler.Models;
using LightSqlProfiler.ViewModels;
using MahApps.Metro.Controls;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace LightSqlProfiler.Views
{
    /// <summary>
    /// Interaction logic for ColumnSelectorWindow.xaml
    /// </summary>
    public partial class ColumnSelectorWindow : MetroWindow
    {
        public ColumnSelectorWindow(IEnumerable<GuiGridColumn> columns, ColumnSelectorVM.OnSaveDelegate onSave)
        {
            InitializeComponent();

            var vm = new ColumnSelectorVM(columns, onSave);
            vm.CloseAction = Close;

            // setup quick-mouse commands
            AvailableBox.MouseDoubleClick += (ss, ee) =>
            {
                var item = Common.FindAncestor<ListBoxItem>(ee.OriginalSource as DependencyObject);
                if (item != null)
                    vm.AddColumnCommand.Execute(item);
            };

            SelectedBox.MouseDoubleClick += (ss, ee) =>
            {
                var item = Common.FindAncestor<ListBoxItem>(ee.OriginalSource as DependencyObject);
                if (item != null)
                    vm.RemoveColumnCommand.Execute(item);
            };

            this.DataContext = vm;
        }
    }
}
