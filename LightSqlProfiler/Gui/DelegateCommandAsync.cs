using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LightSqlProfiler.Gui
{
    /// <summary>
    /// Async version of ICommand extension for GUI commands
    /// </summary>
    public class DelegateCommandAsync : ICommand
    {
        private Predicate<object> _canExecute;
        private Func<object, Task> _execute;

        public DelegateCommandAsync(Predicate<object> canExecute, Func<object, Task> execute)
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
