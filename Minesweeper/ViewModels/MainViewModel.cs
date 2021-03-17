using Minesweeper.Commands;
using Minesweeper.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Minesweeper.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Grid> BoardGrid { get; set; }
        public BaseCommand AddBoardGridCommand { get; set; }
        public GameButtonCommand GameButtonCommand { get; set; }
        public GameButtonRightClickCommand GameButtonRightClickCommand { get; set; }
        public double WindowWidth { get; set; }
        public double WindowHeight { get; set; }
        public double BoardGridWidth { get; set; }
        public double BoardGridHeight { get; set; }
        public int NumberOfRows { get; set; }
        public int NumberOfColumns { get; set; }
        public int NumberOfMines { get; set; }
        public int MinesLeft { get; set; }
        public List<GameField> GameFields { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private bool gameStarted;

        public MainViewModel()
        {
            NumberOfColumns = 9;
            NumberOfRows = 9;
            NumberOfMines = 10;
            MinesLeft = NumberOfMines;
            GameFields = new List<GameField>();
            Grid sampleGrid = new Grid();
            sampleGrid.ShowGridLines = true;
            GameButtonCommand = new GameButtonCommand(this);
            GameButtonRightClickCommand = new GameButtonRightClickCommand(this);
            for (int i = 0; i < 9; i++)
            {
                sampleGrid.ColumnDefinitions.Add(new ColumnDefinition());
                sampleGrid.RowDefinitions.Add(new RowDefinition());
            }
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    Button button = new Button();
                    GameFields.Add(new GameField(button, i, j));
                    button.Name = "r" + i + "c" + j;
                    button.Command = GameButtonCommand;
                    button.CommandParameter = button.Name;
                    MouseGesture mouseGesture = new MouseGesture();
                    mouseGesture.MouseAction = MouseAction.RightClick;
                    MouseBinding mouseBinding = new MouseBinding();
                    mouseBinding.Gesture = mouseGesture;
                    mouseBinding.Command = GameButtonRightClickCommand;
                    mouseBinding.CommandParameter = button.Name;

                    button.InputBindings.Add(mouseBinding);
                    Grid.SetColumn(button, j);
                    Grid.SetRow(button, i);
                    sampleGrid.Children.Add(button);
                }
            }
            BoardGrid = new ObservableCollection<Grid>() { sampleGrid };
            AddBoardGridCommand = new BaseCommand(CreateNewGameBoard);
            gameStarted = false;
        }

        public void SetBoardGridSize()
        {
            if (WindowHeight / NumberOfRows < WindowWidth / NumberOfColumns)
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
            NumberOfRows = new Random().Next(5, 15);
            NumberOfColumns = new Random().Next(5, 15);
            NumberOfMines = new Random().Next(10, (NumberOfRows - 1) * (NumberOfColumns - 1));
            MinesLeft = NumberOfMines;
            PropertyChanged(this, new PropertyChangedEventArgs("MinesLeft"));
            GameFields = new List<GameField>();
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
                    GameFields.Add(new GameField(button, i, j));
                    button.Name = "r" + i + "c" + j;
                    button.Command = GameButtonCommand;
                    button.CommandParameter = button.Name;

                    MouseGesture mouseGesture = new MouseGesture();
                    mouseGesture.MouseAction = MouseAction.RightClick;
                    MouseBinding mouseBinding = new MouseBinding();
                    mouseBinding.Gesture = mouseGesture;
                    mouseBinding.Command = GameButtonRightClickCommand;
                    mouseBinding.CommandParameter = button.Name;

                    button.InputBindings.Add(mouseBinding);

                    Grid.SetColumn(button, j);
                    Grid.SetRow(button, i);
                    grid.Children.Add(button);
                }
            }
            gameStarted = false;
            BoardGrid[0] = grid;
        }

        public void HandleGameFieldClick(GameField gameField)
        {
            if (gameStarted == false)
            {
                gameStarted = true;
                GenerateMines(gameField);
            }
            if(ShowField(gameField) == false)
            {
                RevealAllMines(gameField);
            }
        }

        public void GenerateMines(GameField gameField)
        {
            int minesPlaced = 0;
            var fieldsAroundClickedField = GetFieldsAround(gameField);
            List<GameField> fieldsWithoutMine = GameFields.Where(x => !fieldsAroundClickedField.Contains(x) &&
                                                                 x != gameField).ToList();

            while (minesPlaced < NumberOfMines)
            {
                GameField randomField = fieldsWithoutMine[new Random().Next(fieldsWithoutMine.Count)];
                randomField.IsMine = true;
                minesPlaced++;

                List<GameField> fieldsAround = GetFieldsAround(randomField);
                foreach (var fieldAround in fieldsAround)
                {
                    fieldAround.NumberOfMinesAround++;
                }
                fieldsWithoutMine.Remove(randomField);
            }
        }

        public List<GameField> GetFieldsAround(GameField gameField)
        {
            var list = GameFields.Where(x => x.Column >= gameField.Column - 1 &&
                                             x.Column <= gameField.Column + 1 &&
                                             x.Row >= gameField.Row - 1 &&
                                             x.Row <= gameField.Row + 1).ToList();
            list.Remove(gameField);
            return list;
        }

        public bool ShowField(GameField gameField)
        {
            if (!gameField.IsClickable)
            {
                return true;
            }
            var img = new Image();
            gameField.Button.IsEnabled = false;
            if (gameField.IsMine)
            {
                img.Source = new BitmapImage(new Uri("Resources/mine.png", UriKind.Relative));
                gameField.Button.Content = img;
                return false;
            }
            
            if (gameField.IsMine || gameField.NumberOfMinesAround != 0)
            {
                img.Source = new BitmapImage(new Uri("Resources/" + gameField.NumberOfMinesAround + ".png", UriKind.Relative));
                gameField.Button.Content = img;
                return true;
            }
            var fieldsAround = GetFieldsAround(gameField);
            fieldsAround = fieldsAround.Where(x => x.Button.IsEnabled == true).ToList();
            foreach (var field in fieldsAround)
            {
                ShowField(field);
            }
            return true;
        }

        public void RevealAllMines(GameField gameField)
        {
            var listOfMines = GameFields.Where(x => x.IsMine).ToList();
            foreach (var mine in listOfMines)
            {
                ShowField(mine);
            }
        }

        public void SetUpTheFlag(GameField gameField)
        {
            if (gameField.IsClickable)
            {
                var img = new Image();
                img.Source = new BitmapImage(new Uri("Resources/flag.png", UriKind.Relative));
                gameField.Button.Content = img;
                gameField.IsClickable = false;
                MinesLeft--;
                PropertyChanged(this, new PropertyChangedEventArgs("MinesLeft"));
            }
            else
            {
                gameField.Button.Content = null;
                gameField.IsClickable = true;
                MinesLeft++;
                PropertyChanged(this, new PropertyChangedEventArgs("MinesLeft"));
            }
        }
    }
}
