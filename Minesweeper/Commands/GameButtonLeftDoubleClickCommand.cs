using Minesweeper.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;

namespace Minesweeper.Commands
{
    public class GameButtonLeftDoubleClickCommand : ICommand
    {
        public GameViewModel GameViewModel { get; set; }

        public GameButtonLeftDoubleClickCommand(GameViewModel gameViewModel)
        {
            GameViewModel = gameViewModel;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object sender)
        {
            GameViewModel.HandleDoubleClick(GameViewModel.GameFields.SingleOrDefault(x => x.BottomButton == sender as Button));
        }
    }
}
