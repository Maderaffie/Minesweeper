using Minesweeper.Commands;
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
            _mainWindowViewModel.SelectedViewModel = new GameViewModel(_mainWindowViewModel);
        }

        public void OpenSettingsView()
        {
            _mainWindowViewModel.SelectedViewModel = new GameViewModel(_mainWindowViewModel);
        }

        public void OpenCreditsView()
        {
            _mainWindowViewModel.SelectedViewModel = new GameViewModel(_mainWindowViewModel);
        }
    }
}
