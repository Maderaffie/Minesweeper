using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Input;

namespace Minesweeper.Commands
{
    public class OpenUrlCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            Process process = new Process(); 
            process.StartInfo.UseShellExecute = true;
            process.StartInfo.FileName = parameter as string;
            process.Start();
        }
    }
}
