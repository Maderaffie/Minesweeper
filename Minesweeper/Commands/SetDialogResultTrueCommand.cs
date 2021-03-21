using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Minesweeper.Commands
{
    public class SetDialogResultTrueCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object sender)
        {
            Button button = sender as Button;
            Window parentWindow = Window.GetWindow(button);
            parentWindow.DialogResult = true;
        }
    }
}
