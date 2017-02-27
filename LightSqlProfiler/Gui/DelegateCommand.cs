using System;
using System.Windows.Input;

namespace LightSqlProfiler.Gui
{
    /// <summary>
    /// Synchronize ICommand extension for registering GUI events
    /// </summary>
    public class DelegateCommand : ICommand
    {
        private Predicate<object> _canExecute;
        private Action<object> _execute;

        public DelegateCommand(Predicate<object> canExecute, Action<object> execute)
        {
            this._canExecute = canExecute;
            this._execute = execute;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter) => _canExecute(parameter);

        public void Execute(object parameter) => _execute(parameter);
    }
}
