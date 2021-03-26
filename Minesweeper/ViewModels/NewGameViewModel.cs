using Minesweeper.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Minesweeper.ViewModels
{
    public class NewGameViewModel : BaseViewModel
    {
        private int _numberOfRows;
        public int NumberOfRows
        {
            get { return _numberOfRows; }
            set
            {
                _numberOfRows = value;
                CalculateMaxNumberOfMines();
                OnPropertyChanged(nameof(NumberOfRows));
            }
        }

        private int _numberOfColumns;
        public int NumberOfColumns
        {
            get { return _numberOfColumns; }
            set
            {
                _numberOfColumns = value;
                CalculateMaxNumberOfMines();
                OnPropertyChanged(nameof(NumberOfColumns));
            }
        }

        private int _numberOfMines;
        public int NumberOfMines
        {
            get { return _numberOfMines; }
            set
            {
                _numberOfMines = value;
                OnPropertyChanged(nameof(NumberOfMines));
            }
        }

        private int _maxNumberOfMines;
        public int MaxNumberOfMines
        {
            get { return _maxNumberOfMines; }
            set
            {
                _maxNumberOfMines = value;
                OnPropertyChanged(nameof(MaxNumberOfMines));
            }
        }

        public SetNewGameBoardCommand SetNewGameBoardCommand { get; set; }
        public SetDialogResultTrueCommand SetDialogResultTrueCommand { get; set; }
        public NewGameViewModel()
        {
            SetDialogResultTrueCommand = new SetDialogResultTrueCommand();
            SetNewGameBoardCommand = new SetNewGameBoardCommand(this);
            SetDifficulty();
        }

        public void SetDifficulty(string difficulty = "Beginner")
        {
            switch (difficulty)
            {
                case "Beginner":
                    NumberOfRows = 9;
                    NumberOfColumns = 9;
                    NumberOfMines = 10;
                    break;
                case "Intermediate":
                    NumberOfRows = 16;
                    NumberOfColumns = 16;
                    NumberOfMines = 40;
                    break;
                case "Expert":
                    NumberOfRows = 16;
                    NumberOfColumns = 30;
                    NumberOfMines = 99;
                    break;
                default:
                    NumberOfRows = 9;
                    NumberOfColumns = 9;
                    NumberOfMines = 10;
                    break;
            }
        }

        public void CalculateMaxNumberOfMines()
        {
            var maxNumber = (NumberOfRows - 1) * (NumberOfColumns - 1);
            if (maxNumber < NumberOfMines)
            {
                NumberOfMines = maxNumber;
            }
            MaxNumberOfMines = maxNumber;
        }
    }
}
