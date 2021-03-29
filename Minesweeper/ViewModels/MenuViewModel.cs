using Minesweeper.Commands;
using Minesweeper.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

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
            _mainWindowViewModel.SetGameViewModel();
        }

        public void OpenSettingsView()
        {
            _mainWindowViewModel.SetSettingsViewModel();
        }

        public void OpenCreditsView()
        {
            _mainWindowViewModel.SetCreditsViewModel();
        }
    }
}
