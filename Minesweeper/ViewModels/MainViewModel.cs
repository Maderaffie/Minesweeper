using Minesweeper.Commands;
using Minesweeper.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Minesweeper.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Grid> BoardGrid { get; set; }
        public BaseCommand AddBoardGridCommand { get; set; }
        public GameButtonCommand GameButtonCommand { get; set; }
        public double WindowWidth { get; set; }
        public double WindowHeight { get; set; }
        public double BoardGridWidth { get; set; }
        public double BoardGridHeight { get; set; }
        public int NumberOfRows { get; set; }
        public int NumberOfColumns { get; set; }
        public List<GameField> GameFields { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public MainViewModel()
        {
            NumberOfColumns = 5;
            NumberOfRows = 5;
            GameFields = new List<GameField>();
            Grid sampleGrid = new Grid();
            sampleGrid.ShowGridLines = true;
            GameButtonCommand = new GameButtonCommand(this);
            for (int i = 0; i < 5; i++)
            {
                sampleGrid.ColumnDefinitions.Add(new ColumnDefinition());
                sampleGrid.RowDefinitions.Add(new RowDefinition());
            }
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    Button button = new Button();
                    GameFields.Add(new GameField(button, i, j));
                    button.Name = "r" + i + "c" + j;
                    button.Command = GameButtonCommand;
                    button.CommandParameter = button.Name;
                    Grid.SetColumn(button, j);
                    Grid.SetRow(button, i);
                    sampleGrid.Children.Add(button);
                }
            }
            BoardGrid = new ObservableCollection<Grid>() { sampleGrid };
            AddBoardGridCommand = new BaseCommand(CreateNewGameBoard);

        }

        public void SetBoardGridSize()
        {
            if (WindowHeight/NumberOfRows < WindowWidth/NumberOfColumns)
            {
                double cellSize = (WindowHeight - 30) / NumberOfRows;
                BoardGridHeight = cellSize * NumberOfRows;
                BoardGridWidth = cellSize * NumberOfColumns;
            }
            else
            {
                double cellSize = (WindowWidth - 30) / NumberOfColumns;
                BoardGridHeight = cellSize * NumberOfRows;
                BoardGridWidth = cellSize * NumberOfColumns;
            }
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs("BoardGridHeight"));
                PropertyChanged(this, new PropertyChangedEventArgs("BoardGridWidth"));
            }
        }

        public void OnWindowSizeChanged(object sender, SizeChangedEventArgs e)
        {
            WindowHeight = e.NewSize.Height - 70;
            WindowWidth = e.NewSize.Width;
            SetBoardGridSize();
        }

        public void CreateNewGameBoard()
        {
            Grid grid = new Grid();
            grid.ShowGridLines = true;
            NumberOfRows = new Random().Next(2, 15);
            NumberOfColumns = new Random().Next(2, 15);
            SetBoardGridSize();
            for (int i = 0; i < NumberOfRows; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition());
            }
            for (int i = 0; i < NumberOfColumns; i++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition());
            }
            for (int i = 0; i < NumberOfRows; i++)
            {
                for (int j = 0; j < NumberOfColumns; j++)
                {
                    Button button = new Button();
                    Grid.SetColumn(button, j);
                    Grid.SetRow(button, i);
                    grid.Children.Add(button);
                }
            }
            BoardGrid[0] = grid;
        }

        public void HandleGameFieldClick(GameField gameField)
        {
            Debug.WriteLine("I got " + gameField.Name);
        }
    }
}
