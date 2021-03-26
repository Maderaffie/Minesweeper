using Minesweeper.Commands;
using Minesweeper.Views;
using System;
using System.Collections.Generic;
using System.Text;

namespace Minesweeper.ViewModels
{
    public class MenuViewModel : BaseViewModel
    {
        private readonly MainWindowViewModel _mainWindowViewModel;
        public BaseCommand OpenGameViewCommand { get; set; }
        public BaseCommand OpenSettingsViewCommand { get; set; }
        public BaseCommand OpenCreditsViewCommand { get; set; }
        public MenuViewModel(MainWindowViewModel mainWindowViewModel)
        {
            _mainWindowViewModel = mainWindowViewModel;
            OpenCreditsViewCommand = new BaseCommand(OpenCreditsView);
            OpenGameViewCommand = new BaseCommand(OpenGameView);
            OpenSettingsViewCommand = new BaseCommand(OpenSettingsView);
        }

        public void OpenGameView()
        {
            var newGameDialog = new NewGameDialog();
            var newGameViewModel = new NewGameViewModel();
            newGameDialog.DataContext = newGameViewModel;
            if (newGameDialog.ShowDialog() == true)
            {
                var gameViewModel = new GameViewModel(_mainWindowViewModel, newGameViewModel.NumberOfRows, newGameViewModel.NumberOfColumns, newGameViewModel.NumberOfMines);
                _mainWindowViewModel.SelectedViewModel = gameViewModel;
            }
        }

        public void OpenSettingsView()
        {
            _mainWindowViewModel.SelectedViewModel = new GameViewModel(_mainWindowViewModel, 9, 9, 10);
        }

        public void OpenCreditsView()
        {
            _mainWindowViewModel.SelectedViewModel = new GameViewModel(_mainWindowViewModel, 9, 9, 10);
        }
    }
}
