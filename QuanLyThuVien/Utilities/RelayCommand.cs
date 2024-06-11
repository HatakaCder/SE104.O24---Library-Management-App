using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace QuanLyThuVien.Utilities
{
    public class RelayCommand : ICommand
    {
        // Fields to store the actions
        private readonly Action<object> _execute;
        private readonly Func<object, bool> _canExecute;
        private readonly Action _doWork;

        // Event to handle CanExecute changes
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested += value; }
        }

        // Constructor for commands with parameters
        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        // Constructor for parameterless commands
        public RelayCommand(Action work)
        {
            _doWork = work;
        }

        // Method to determine if the command can be executed
        public bool CanExecute(object parameter) => _canExecute == null || _canExecute(parameter);

        // Method to execute the command
        public void Execute(object parameter)
        {
            if (_execute != null)
            {
                _execute(parameter);
            }
            else if (_doWork != null)
            {
                _doWork();
            }
        }
    }
}
