using Minesweeper.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;

namespace Minesweeper.Commands
{
    public class GameButtonCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        public MainViewModel MainViewModel { get; set; }
        public Button ClickedButton { get; set; }

        public GameButtonCommand(MainViewModel mainViewModel)
        {
            MainViewModel = mainViewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object sender)
        {
            MainViewModel.HandleGameFieldClick(MainViewModel.GameFields.SingleOrDefault(x => x.Name == sender as string));
        }
    }
}
