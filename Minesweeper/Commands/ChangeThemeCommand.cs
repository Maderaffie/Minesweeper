using Minesweeper.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;

namespace Minesweeper.Commands
{
    public class ChangeThemeCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private SettingsViewModel _settingsViewModel;

        public ChangeThemeCommand(SettingsViewModel settingsViewModel)
        {
            _settingsViewModel = settingsViewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _settingsViewModel.ChangeTheme((parameter as RadioButton).Name);
        }
    }
}
