using LightSqlProfiler.Core;
using LightSqlProfiler.Core.Enums;
using LightSqlProfiler.ViewModels;
using MahApps.Metro.Controls;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace LightSqlProfiler.Views
{
    /// <summary>
    /// Interaction logic for ProfilerPropertiesWindow.xaml
    /// </summary>
    public partial class EventSelectorWindow : MetroWindow
    {
        /// <summary>
        /// On window load, prepare profiler defaults
        /// </summary>
        public EventSelectorWindow(List<EventClassType> selectedEventTypes)
        {
            InitializeComponent();

            EventSelectorVM vm = new EventSelectorVM(
                selectedEventTypes,

                // onSave
                (result) =>
                {
                    selectedEventTypes.Clear();
                    selectedEventTypes.AddRange(result);
                    this.DialogResult = true;
                },

                // onClose
                () => this.DialogResult = false
            );

            // setup quick-mouse commands
            AvailableBox.MouseDoubleClick += (ss, ee) =>
            {
                var item = Common.FindAncestor<ListBoxItem>(ee.OriginalSource as DependencyObject);
                if (item != null)
                    vm.AddEventCommand.Execute(item);
            };

            SelectedBox.MouseDoubleClick += (ss, ee) =>
            {
                var item = Common.FindAncestor<ListBoxItem>(ee.OriginalSource as DependencyObject);
                if (item != null)
                    vm.RemoveEventCommand.Execute(item);
            };

            this.DataContext = vm;
        }
    }
}
