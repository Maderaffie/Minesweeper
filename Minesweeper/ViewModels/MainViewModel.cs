using Minesweeper.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Controls;

namespace Minesweeper.ViewModels
{
    public class MainViewModel
    {
        public ObservableCollection<Grid> BoardGrid { get; set; }
        public BaseCommand AddBoardGridCommand { get; set; }
        public MainViewModel()
        {
            Grid sampleGrid = new Grid();
            sampleGrid.ShowGridLines = true;
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
                    Grid.SetColumn(button, j);
                    Grid.SetRow(button, i);
                    sampleGrid.Children.Add(button);
                }
            }
            BoardGrid = new ObservableCollection<Grid>() { sampleGrid };
            AddBoardGridCommand = new BaseCommand(CreateNewGameBoard);
        }

        public void CreateNewGameBoard()
        {
            Grid grid = new Grid();
            grid.ShowGridLines = true;
            int numberOfRows = new Random().Next(2, 15);
            int numberOfColumns = new Random().Next(2, 15);
            for (int i = 0; i < numberOfRows; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition());
            }
            for (int i = 0; i < numberOfColumns; i++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition());
            }
            for (int i = 0; i < numberOfRows; i++)
            {
                for (int j = 0; j < numberOfColumns; j++)
                {
                    Button button = new Button();
                    Grid.SetColumn(button, j);
                    Grid.SetRow(button, i);
                    grid.Children.Add(button);
                }
            }
            BoardGrid[0] = grid;
        }
    }
}
