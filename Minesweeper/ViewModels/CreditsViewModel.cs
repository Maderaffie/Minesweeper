using Minesweeper.Commands;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Minesweeper.ViewModels
{
    public class CreditsViewModel : BaseViewModel
    {
        private MainWindowViewModel _mainWindowViewModel;
        public BaseCommand BackToMainMenuCommand { get; set; }
        public OpenUrlCommand OpenUrlCommand { get; set; }
        public CreditsViewModel(MainWindowViewModel mainWindowViewModel)
        {
            _mainWindowViewModel = mainWindowViewModel;
            BackToMainMenuCommand = new BaseCommand(ChangeSelectedViewModel);
            OpenUrlCommand = new OpenUrlCommand();
        }

        public void ChangeSelectedViewModel()
        {
            _mainWindowViewModel.SetMenuViewModel();
        }
    }
}
