using System;
using System.Collections.Generic;
using System.Text;

namespace Minesweeper.ViewModels
{
    public class MenuViewModel : BaseViewModel
    {
        private readonly MainWindowViewModel _mainWindowViewModel;
        public MenuViewModel(MainWindowViewModel mainWindowViewModel)
        {
            _mainWindowViewModel = mainWindowViewModel;
        }
    }
}
