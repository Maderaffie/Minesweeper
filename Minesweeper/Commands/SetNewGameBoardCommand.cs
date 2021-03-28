using Minesweeper.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;

namespace Minesweeper.Commands
{
    public class SetNewGameBoardCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private readonly NewGameViewModel _newGameViewModel;
        public SetNewGameBoardCommand(NewGameViewModel newGameViewModel)
        {
            _newGameViewModel = newGameViewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _newGameViewModel.SetDifficulty((parameter as RadioButton).Name);
        }
    }
}
