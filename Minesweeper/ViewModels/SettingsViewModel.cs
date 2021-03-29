using System;
using System.Collections.Generic;
using System.Text;

namespace Minesweeper.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        private MainWindowViewModel _mainWindowViewModel;
        public SettingsViewModel(MainWindowViewModel mainWindowViewModel)
        {
            _mainWindowViewModel = mainWindowViewModel;
        }
    }
}
