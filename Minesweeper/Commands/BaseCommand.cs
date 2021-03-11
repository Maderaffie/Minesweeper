using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Minesweeper.Commands
{
    public class BaseCommand : ICommand
    {
        public Action Action { get; set; }
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            Action?.Invoke();
        }

        public BaseCommand(Action action)
        {
            Action = action;
        }
    }
}
