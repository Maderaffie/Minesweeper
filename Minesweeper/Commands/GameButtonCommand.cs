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
        public GameViewModel GameViewModel { get; set; }
 
        public GameButtonCommand(GameViewModel gameViewModel)
        {
            GameViewModel = gameViewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object sender)
        {
            GameViewModel.HandleGameFieldClick(GameViewModel.GameFields.SingleOrDefault(x => x.Name == sender as string));
        }
    }
}   
