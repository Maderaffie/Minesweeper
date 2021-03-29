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
            GradientLeft = (Color)ColorConverter.ConvertFromString("#4568DC");
            GradientRight = (Color)ColorConverter.ConvertFromString("#B06AB3");
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
            // TODO: Create settings viewmodel 
            SelectedViewModel = new SettingsViewModel(this);
        }

        public void SetCreditsViewModel()
        {
            // TODO: Create credits viewmodel 
            return;
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
