using Minesweeper.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace Minesweeper.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        private string _theme;
        public string Theme
        {
            get { return _theme; }
            set
            {
                _theme = value;
                OnPropertyChanged(nameof(Theme));
            }
        }

        public ChangeThemeCommand ChangeThemeCommand { get; set; }
        public BaseCommand BackToMainMenuCommand { get; set; }
        private readonly MainWindowViewModel _mainWindowViewModel;
        public SettingsViewModel(MainWindowViewModel mainWindowViewModel)
        {
            _mainWindowViewModel = mainWindowViewModel;
            ChangeThemeCommand = new ChangeThemeCommand(this);
            BackToMainMenuCommand = new BaseCommand(ChangeSelectedViewModel);
        }

        public void ChangeTheme(string themeName)
        {
            switch (themeName)
            {
                case "CanYouFeelTheLoveTonight":
                    SetNewGradient("#4568DC", "#B06AB3");
                    break;
                case "CoolBlues":
                    SetNewGradient("#2193b0", "#6dd5ed");
                    break;
                case "WitchingHour":
                    SetNewGradient("#c31432", "#240b36");
                    break;
                default:
                    break;
            }
            Theme = themeName;
        }

        public void SetNewGradient(string leftColor, string rightColor)
        {
            Properties.Settings.Default.LeftGradient = (Color)ColorConverter.ConvertFromString(leftColor);
            Properties.Settings.Default.RightGradient = (Color)ColorConverter.ConvertFromString(rightColor);
            _mainWindowViewModel.SetNewGradient();
        }

        public void ChangeSelectedViewModel()
        {
            _mainWindowViewModel.SetMenuViewModel();
        }
    }
}
