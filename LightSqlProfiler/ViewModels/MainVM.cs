using LightSqlProfiler.Core;
using LightSqlProfiler.Core.AutoUpdate;
using LightSqlProfiler.Core.Enums;
using LightSqlProfiler.Core.Executor;
using LightSqlProfiler.Core.Trace;
using LightSqlProfiler.Gui;
using LightSqlProfiler.Models;
using LightSqlProfiler.Views;
using log4net;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LightSqlProfiler.ViewModels
{
    internal class MainVM : ObservableObject
    {
        private static readonly ILog Log = LogManager.GetLogger(nameof(MainVM));

        /// <summary>
        /// Traces/connections manager
        /// </summary>
        private TraceManager Traces = new TraceManager();

        /// <summary>
        /// Background job cancellation token (cancels async operations)
        /// </summary>
        private CancellationTokenSource _cancelSource;

        /// <summary>
        /// Global user/app settings
        /// </summary>
        public UserSettings Settings { get; set; }

        /// <summary>
        /// Current application status tracker
        /// </summary>
        public AppState Status { get; set; } = new AppState();

        /// <summary>
        /// All captured trace events listed in the table
        /// </summary>
        public ObservableCollection<ProfilerEvent> Events { get; set; } = new ObservableCollection<ProfilerEvent>();

        #region GUI selections

        /// <summary>
        /// Currently selected trace event
        /// Updates SQL Preview box when updating
        /// </summary>
        public ProfilerEvent SelectedEvent
        {
            get
            {
                return _selectedEvent;
            }
            set
            {
                _selectedEvent = value;

                // grab text data from TextData column (if column is available and event is selected)
                var sqlText = _selectedEvent?.HasColumn(EventColumnType.TextData) == true
                    ? _selectedEvent.GetValue(EventColumnType.TextData)?.ToString()
                    : string.Empty;

                SqlPreview.SetText(sqlText);

                // try to get the database name
                CurrentDatabaseName = _selectedEvent?.HasColumn(EventColumnType.TextData) == true
                    ? _selectedEvent?.GetValue(EventColumnType.DatabaseName)?.ToString()
                    : null;

                OnPropertyChanged();
            }
        }

        private ProfilerEvent _selectedEvent;

        private string _currentDatabaseName { get; set; }
        public string CurrentDatabaseName
        {
            get { return _currentDatabaseName; }
            set { _currentDatabaseName = value; OnPropertyChanged(); }
        }


        #endregion GUI selections

        #region Commands

        public ICommand StartCommand { get; set; }
        public ICommand StopCommand { get; set; }
        public ICommand ServersCommand { get; set; }
        public ICommand ClearEventsCommand { get; set; }
        public ICommand SelectColumnsCommand { get; set; }
        public ICommand CopySqlCommand { get; set; }
        public ICommand UserSettingsCommand { get; set; }
        public ICommand AboutCommand { get; set; }
        public ICommand CheckUpdatesCommand { get; set; }
        public ICommand ExitCommand { get; set; }
        public ICommand RunCommand { get; set; }
        public ICommand RunBackgroundSqlCommand { get; set; }

        #endregion Commands

        #region GUI control handlers

        public SqlPreviewHandler SqlPreview { get; set; }
        public EventViewHandler EventGrid { get; set; }

        #endregion GUI control handlers

        public MainVM()
        {
            StartCommand = new DelegateCommandAsync(
                o => Settings.Connections.CurrentConnection != null && Status.Status == AppStatusCodes.Ready,
                o =>
                {
                    // show server menu when we need a password or connection
                    if ((o as bool?) != true && Settings.Connections.CurrentConnection?.RawPassword == null)
                    {
                        ServersCommand.Execute(null);
                        return Task.FromResult<object>(null);
                    }

                    return StartTraceAsync();
                }
            );

            StopCommand = new DelegateCommandAsync(
                o => Status.Status != AppStatusCodes.Ready,
                o => StopTraceAsync()
            );

            ServersCommand = new DelegateCommand(
                o => Status.Status == AppStatusCodes.Ready,
                o => new ConnectWindow(Settings.Connections, () => StartCommand.Execute(true)).Show()
            );

            ClearEventsCommand = new DelegateCommand(
                o => true,
                o => Events.Clear()
            );

            SelectColumnsCommand = new DelegateCommand(
                o => true,
                o =>
                {
                    new ColumnSelectorWindow(
                        Settings.Columns,
                        (result) =>
                        {
                            EventGrid.BuildColumns(result, Events);
                            Settings.Columns = result.ToList();
                            OnPropertyChanged(nameof(Settings));
                        }
                    ).Show();
                }
            );

            CopySqlCommand = new DelegateCommand(
                o => true,
                o => SqlPreview.CopyToClipboard()
            );

            UserSettingsCommand = new DelegateCommand(
                o => true,
                o => new UserSettingsWindow(Settings).Show()
            );

            AboutCommand = new DelegateCommand(
                o => true,
                o => new AboutWindow().Show()
            );

            CheckUpdatesCommand = new DelegateCommandAsync(
                o => true,
                o => CheckForUpdates()
            );

            ExitCommand = new DelegateCommand(
                o => true,
                o => OnExit()
            );

            RunCommand = new DelegateCommand(
                o => Status.Status == AppStatusCodes.Running && SelectedEvent?.EventType != EventClassType.Custom  && SqlPreview?.GetText() != null,
                o => RunSqlCode()
            );

            RunBackgroundSqlCommand = new DelegateCommandAsync(
                o => Status.Status == AppStatusCodes.Running && SelectedEvent?.EventType != EventClassType.Custom && SqlPreview?.GetText() != null,
                o =>
                {
                    var exec = new ExecQuery((ServerConnection)Status.ActiveConnection.Clone(), CurrentDatabaseName);
                    return exec.ExecSqlAsync(SqlPreview.GetText());
                }
            );

            // capture new events
            Traces.OnEvent += (status, ev) =>
            {
                if (status == ProfilerEventStatus.NewEvent && !ev.IsInternal)
                    Events.Add(ev);
            };

            // update canExecute() for each command when status changes
            Status.PropertyChanged += (ee, ss) => StatusChanged();
        }

        private void RunSqlCode()
        {
            new QueryWindow((ServerConnection)Status.ActiveConnection.Clone(), CurrentDatabaseName, SqlPreview.GetText()).Show();
        }

        /// <summary>
        /// Externally invoked method signaling that application is about to exit
        /// Cleanup any external connection and save current settings
        /// </summary>
        public void OnExit()
        {
            Status.IsExiting = true;
            if (Status.Status != AppStatusCodes.Ready)
            {
                Log.Debug("Application closing while connection exists. Closing connection first.");
                Task.Run(async () => await StopTraceAsync()).Wait();
            }
            Settings.SaveSettings();

            Application.Current.Shutdown();
        }

        private Task CheckForUpdates()
        {
            return Task.Run(() =>
            {
                var update = new AutoUpdateService();
                var isUpdateAvailable = update.IsUpdateAvailable() == true;
                if (isUpdateAvailable)
                {
                    if (MessageBox.Show("New version available. Do you want to download it now?", "Update available",
                        MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        update.DownloadAndRun();
                    }
                }
                else
                    MessageBox.Show("You are using latest version.", "No updates available", MessageBoxButton.OK, MessageBoxImage.Information);
            });
        }

        private async Task StartTraceAsync()
        {
            // trace options window
            var optionsWnd = new EventSelectorWindow(Settings.Events);
            if (optionsWnd.ShowDialog() != true)
                return;

            Status.Status = AppStatusCodes.Connecting;
            try
            {
                _cancelSource = new CancellationTokenSource();
                var registeredEvents = Common.MapRegisteredColumnsToEvents(Settings.Events, Settings.Columns.Select(x => x.ColumnType));
                await Traces.StartTraceSessionAsync(Settings.Connections.CurrentConnection, registeredEvents, _cancelSource.Token);
                Status.Status = AppStatusCodes.Running;
                Status.ActiveConnection = Settings.Connections.CurrentConnection;
            }
            // during canceled async, SQL server itself throws an exception, which we want to suppress
            catch (Exception ex) when (ex?.Message?.Contains("cancel") == true)
            {
                Log.Debug("SQL execution canceled by user");
            }
            catch (OperationCanceledException)
            {
                Log.Debug("Connection canceled by user");
            }
            catch (Exception ex)
            {
                Log.Warn("Unexpected connection error", ex);
                MessageBox.Show(ex.Message, "DB connection error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            // if execution haven't set this to running (exception happened), reset the status to "Ready" state
            if (Status.Status != AppStatusCodes.Running)
            {
                Status.Status = AppStatusCodes.Ready;
                Status.ActiveConnection = null;
            }
        }

        private async Task StopTraceAsync()
        {
            // check if trace is registered: if it is - we need to unregister it first, before disconnecting
            // otherwise, a simple cancel-connection operation needs to be issues
            // check status before updating it to "Disconnecting" state
            var isTraceRunning = Status.Status == AppStatusCodes.Running;

            Status.Status = AppStatusCodes.Disconnecting;
            Status.ActiveConnection = null;
            if (!isTraceRunning)
            {
                Log.Debug("StopTrace: Still connecting (trace has not started), aborting DB connection");
                _cancelSource.Cancel();
            }
            else
            {
                Log.Debug("StopTrace(): Trace already running. Stopping trace before disconnecting");
                await Traces.StopTraceSessionAsync(_cancelSource.Token);
            }
            Status.Status = AppStatusCodes.Ready;
        }

        private void StatusChanged()
        {
            if (Status.IsExiting)
                return;

            // update ICommand
            CommandManager.InvalidateRequerySuggested();

            // insert custom events

            if (Settings.App.AddTraceStartEvent && Status.Status == AppStatusCodes.Running)
            {
                var item = new ProfilerEvent(EventClassType.Custom);
                item.SetColumnValue(EventColumnType.TextData, "Trace started");
                Events.Add(item);
            }

            if (Settings.App.AddTraceStopEvent && Status.Status == AppStatusCodes.Ready)
            {
                var item = new ProfilerEvent(EventClassType.Custom);
                item.SetColumnValue(EventColumnType.TextData, "Trace stoped");
                Events.Add(item);
            }
        }
    }
}
