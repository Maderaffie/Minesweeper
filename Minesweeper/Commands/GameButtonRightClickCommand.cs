using Minesweeper.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace Minesweeper.Commands
{
    public class GameButtonRightClickCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        public MainViewModel MainViewModel { get; set; }

        public GameButtonRightClickCommand(MainViewModel mainViewModel)
        {
            MainViewModel = mainViewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object sender)
        {
            MainViewModel.SetUpTheFlag(MainViewModel.GameFields.SingleOrDefault(x => x.Name == sender as string));
        }
    }
}
