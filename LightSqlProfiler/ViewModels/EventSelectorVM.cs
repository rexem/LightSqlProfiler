using LightSqlProfiler.Core;
using LightSqlProfiler.Core.Enums;
using LightSqlProfiler.Core.Trace.Definitions;
using LightSqlProfiler.Core.Trace.Entities;
using LightSqlProfiler.Gui;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace LightSqlProfiler.ViewModels
{
    public class EventSelectorVM : ObservableObject
    {
        public ObservableCollection<TraceEvent> AvailableEvents { get; set; } = new ObservableCollection<TraceEvent>();

        private TraceEvent _selectedAvailableEvent;

        public TraceEvent SelectedAvailableEvent
        {
            get { return _selectedAvailableEvent; }
            set { _selectedAvailableEvent = value; OnPropertyChanged(); }
        }

        public ObservableCollection<TraceEvent> SelectedEvents { get; set; } = new ObservableCollection<TraceEvent>();

        private TraceEvent _selectedSelectedEvent;

        public TraceEvent SelectedSelectedEvent
        {
            get { return _selectedSelectedEvent; }
            set { _selectedSelectedEvent = value; OnPropertyChanged(); }
        }

        #region Commands

        public ICommand CloseCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand AddEventCommand { get; }
        public ICommand RemoveEventCommand { get; }
        public ICommand ResetDefaultsCommand { get; }

        #endregion Commands

        public EventSelectorVM(IEnumerable<EventClassType> events, Action<IEnumerable<EventClassType>> onSave, Action onClose)
        {
            CloseCommand = new DelegateCommand(
                o => true,
                o => onClose()
            );

            SaveCommand = new DelegateCommand(
                o => true,
                o => onSave(SelectedEvents.Select(x => x.EventClass))
            );

            AddEventCommand = new DelegateCommand(
                o => SelectedAvailableEvent != null,
                o => AddEvent(SelectedAvailableEvent)
            );

            RemoveEventCommand = new DelegateCommand(
                o => SelectedSelectedEvent != null,
                o => RemoveEvent(SelectedSelectedEvent)
            );

            ResetDefaultsCommand = new DelegateCommand(
                o => true,
                o => ResetDefaults()
            );

            SetEvents(events);
        }

        private void SetEvents(IEnumerable<EventClassType> selectedEventTypes)
        {
            AvailableEvents.Clear();
            foreach (var item in EventDefinition.Instance.Events)
                AvailableEvents.Add(item);

            SelectedEvents.Clear();
            foreach (var item in EventDefinition.Instance.Events.Where(x => selectedEventTypes.Contains(x.EventClass)))
                AddEvent(item);

            SelectedSelectedEvent = SelectedAvailableEvent = null;
        }

        private void AddEvent(TraceEvent ev)
        {
            SelectedEvents.Add(ev);
            AvailableEvents.Remove(ev);
            SelectedSelectedEvent = ev;
            SelectedAvailableEvent = null;
        }

        private void RemoveEvent(TraceEvent ev)
        {
            AvailableEvents.Add(ev);
            SelectedEvents.Remove(ev);
            SelectedAvailableEvent = ev;
            SelectedSelectedEvent = null;
        }

        private void ResetDefaults()
        {
            var items = new UserSettings().GetDefaultEvents();
            SetEvents(items);
        }
    }
}
