using Minesweeper.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

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

        public MainWindowViewModel() { }

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
            return;
        }

        public void SetCreditsViewModel()
        {
            // TODO: Create credits viewmodel 
            return;
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
