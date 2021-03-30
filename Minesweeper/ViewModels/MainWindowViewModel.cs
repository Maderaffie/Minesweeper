using Minesweeper.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace Minesweeper.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        private BaseViewModel _selectedViewModel;
        public BaseViewModel SelectedViewModel
        {
            get { return _selectedViewModel; }
            set
            {
                _selectedViewModel = value;
                OnPropertyChanged(nameof(SelectedViewModel));
            }
        }

        private Color _gradientLeft;
        public Color GradientLeft
        {
            get { return _gradientLeft; }
            set
            {
                _gradientLeft = value;
                OnPropertyChanged(nameof(GradientLeft));
            }
        }
        private Color _gradientRight;
        public Color GradientRight
        {
            get { return _gradientRight; }
            set
            {
                _gradientRight = value;
                OnPropertyChanged(nameof(GradientRight));
            }
        }

        public MainWindowViewModel()
        {
            SetNewGradient();
        }

        public void SetNewGradient()
        {
            GradientLeft = Properties.Settings.Default.LeftGradient;
            GradientRight = Properties.Settings.Default.RightGradient;
            Properties.Settings.Default.Save();
        }

        public void SetGameViewModel()
        {
            var newGameViewModel = OpenNewGameDialog();
            if (newGameViewModel != null)
            {
                var gameViewModel = new GameViewModel(this, newGameViewModel.NumberOfRows, newGameViewModel.NumberOfColumns, newGameViewModel.NumberOfMines);
                SelectedViewModel = gameViewModel;
            }
        }

        public void SetSettingsViewModel()
        {
            SelectedViewModel = new SettingsViewModel(this);
        }

        public void SetCreditsViewModel()
        {
            SelectedViewModel = new CreditsViewModel(this);
        }

        public void SetMenuViewModel()
        {
            SelectedViewModel = new MenuViewModel(this);
        }

        public NewGameViewModel OpenNewGameDialog()
        {
            var newGameDialog = new NewGameDialog();
            var newGameViewModel = new NewGameViewModel();
            newGameDialog.Owner = Application.Current.MainWindow;
            newGameDialog.DataContext = newGameViewModel;
            if (newGameDialog.ShowDialog() == true)
            {
                return newGameViewModel;
            }
            else
            {
                return null;
            }
        }
    }
}
