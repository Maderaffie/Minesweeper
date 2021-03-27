using Minesweeper.Commands;
using Minesweeper.Models;
using Minesweeper.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Threading;
using System.Xml;

namespace Minesweeper.ViewModels
{
    public class GameViewModel : BaseViewModel
    {
        public ObservableCollection<Grid> BoardGrid { get; set; }
        public BaseCommand AddBoardGridCommand { get; set; }
        public GameButtonCommand GameButtonCommand { get; set; }
        public GameButtonRightClickCommand GameButtonRightClickCommand { get; set; }
        public GameButtonLeftDoubleClickCommand GameButtonLeftDoubleClickCommand { get; set; }
        public int NumberOfRows { get; set; }
        public int NumberOfColumns { get; set; }
        public int NumberOfMines { get; set; }
        public int FlagsLeft { get; set; }
        public List<GameField> GameFields { get; set; }
        public string Time { get; set; }
        public Action CloseAction { get; set; }

        private readonly MainWindowViewModel _mainWindowViewModel;
        private DispatcherTimer DispatcherTimer { get; set; }
        private DateTime StartTime { get; set; }
        private bool gameStarted;

        public GameViewModel(MainWindowViewModel mainWindowViewModel, int rows, int columns, int mines)
        {
            _mainWindowViewModel = mainWindowViewModel;
            NumberOfRows = rows;
            NumberOfColumns = columns;
            NumberOfMines = mines;
            GameButtonCommand = new GameButtonCommand(this);
            GameButtonRightClickCommand = new GameButtonRightClickCommand(this);
            GameButtonLeftDoubleClickCommand = new GameButtonLeftDoubleClickCommand(this);

            AddBoardGridCommand = new BaseCommand(StartNewGame);
            StartNewGame();
        }

        public void StartNewGame()
        {
            gameStarted = false;
            Time = "00:00";
            OnPropertyChanged("Time");
            DispatcherTimer = new DispatcherTimer();
            DispatcherTimer.Tick += DispatcherTimer_Tick;
            DispatcherTimer.Interval = TimeSpan.FromSeconds(1);
            GameFields = new List<GameField>();
            CreateNewGameBoard();
        }

        public void CreateNewGameBoard()
        {
            CreateNewGameBoard(NumberOfRows, NumberOfColumns, NumberOfMines);
        }

        public void CreateNewGameBoard(int rows, int columns, int mines)
        {
            Grid grid = new Grid();
            grid.ShowGridLines = true;
            NumberOfRows = rows;
            NumberOfColumns = columns;
            NumberOfMines = mines;

            grid.Height = NumberOfRows * 100;
            grid.Width = NumberOfColumns * 100;
            FlagsLeft = NumberOfMines;
            OnPropertyChanged("FlagsLeft");
            GameFields = new List<GameField>();
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
                    Button bottomButton = new Button();
                    bottomButton.Background = Brushes.Transparent;
                    bottomButton.Name = "r" + i + "c" + j;

                    MouseGesture mouseGesture = new MouseGesture();
                    mouseGesture.MouseAction = MouseAction.LeftDoubleClick;
                    MouseBinding mouseBinding = new MouseBinding();
                    mouseBinding.Gesture = mouseGesture;
                    mouseBinding.Command = GameButtonLeftDoubleClickCommand;
                    mouseBinding.CommandParameter = bottomButton.Name;
                    bottomButton.Style = Application.Current.MainWindow.Resources["ButtonNoHover"] as Style;

                    bottomButton.InputBindings.Add(mouseBinding);

                    Button topButton = new Button();
                    GameFields.Add(new GameField(topButton, bottomButton, i, j));
                    topButton.Name = bottomButton.Name;
                    topButton.Command = GameButtonCommand;
                    topButton.CommandParameter = topButton.Name;

                    mouseGesture = new MouseGesture();
                    mouseGesture.MouseAction = MouseAction.RightClick;
                    mouseBinding = new MouseBinding();
                    mouseBinding.Gesture = mouseGesture;
                    mouseBinding.Command = GameButtonRightClickCommand;
                    mouseBinding.CommandParameter = topButton.Name;

                    topButton.InputBindings.Add(mouseBinding);

                    Grid.SetColumn(bottomButton, j);
                    Grid.SetRow(bottomButton, i);
                    grid.Children.Add(bottomButton);

                    Grid.SetColumn(topButton, j);
                    Grid.SetRow(topButton, i);
                    grid.Children.Add(topButton);
                }
            }
            gameStarted = false;
            if (BoardGrid == null)
            {
                BoardGrid = new ObservableCollection<Grid>();
                BoardGrid.Add(grid);
            }
            else
            {
                BoardGrid[0] = grid;
            }
        }

        public void HandleGameFieldClick(GameField gameField)
        {
            if (gameStarted == false)
            {
                gameStarted = true;
                GenerateMines(gameField);
                StartTime = DateTime.Now;
                DispatcherTimer.Start();
            }
            if (ShowField(gameField) == false)
            {
                RevealAllMines(gameField);
                EndTheGame(false);
            }
            else
            {
                CheckForGameEnd();
            }
        }

        public void HandleDoubleClick(GameField gameField)
        {
            bool noMinesRevealed = true;
            var fieldsAround = GetFieldsAround(gameField);
            if (!(fieldsAround.Where(x => x.IsFlagged == true).ToList().Count == gameField.NumberOfMinesAround))
            {
                return;
            }
            fieldsAround = fieldsAround.Where(x => x.TopButton.IsVisible == true && x.IsFlagged == false).ToList();
            foreach (var field in fieldsAround)
            {
                if (ShowField(field) == false)
                {
                    noMinesRevealed = false;
                }
            }
            if (!noMinesRevealed)
            {
                RevealAllMines(gameField);
                EndTheGame(false);
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
            if (gameField.IsFlagged && gameStarted)
            {
                return true;
            }
            Viewbox viewbox = new Viewbox();
            //gameField.TopButton.IsEnabled = false;
            gameField.TopButton.Visibility = Visibility.Hidden;
            BoardGrid[0].Children.Remove(gameField.TopButton);
            if (gameField.IsMine && !gameField.IsFlagged)
            {
                FileInfo file = new FileInfo("Resources/mine.xaml");
                XmlReader xmlReader = XmlReader.Create(file.FullName);
                Canvas userControl = (Canvas)XamlReader.Load(xmlReader);
                viewbox.Child = userControl;
                gameField.BottomButton.Content = viewbox;
                return false;
            }
            else if (gameField.IsMine && gameField.IsFlagged)
            {
                FileInfo file = new FileInfo("Resources/mine_win.xaml");
                XmlReader xmlReader = XmlReader.Create(file.FullName);
                Canvas userControl = (Canvas)XamlReader.Load(xmlReader);
                viewbox.Child = userControl;
                gameField.BottomButton.Content = viewbox;
                return false;
            }

            if (gameField.IsMine || gameField.NumberOfMinesAround != 0)
            {
                FileInfo file = new FileInfo("Resources/" + gameField.NumberOfMinesAround + ".xaml");
                XmlReader xmlReader = XmlReader.Create(file.FullName);
                Canvas userControl = (Canvas)XamlReader.Load(xmlReader);
                viewbox.Child = userControl;
                gameField.BottomButton.Content = viewbox;
                return true;
            }
            var fieldsAround = GetFieldsAround(gameField);
            fieldsAround = fieldsAround.Where(x => x.TopButton.IsVisible == true).ToList();
            foreach (var field in fieldsAround)
            {
                ShowField(field);
            }
            return true;
        }

        public void RevealAllMines(GameField gameField)
        {
            gameStarted = false;
            var listOfMines = GameFields.Where(x => x.IsMine).ToList();
            foreach (var mine in listOfMines)
            {
                ShowField(mine);
            }
        }

        public void SetUpTheFlag(GameField gameField)
        {
            if (!gameStarted)
            {
                return;
            }
            if (!gameField.IsFlagged)
            {
                Viewbox viewbox = new Viewbox();
                FileInfo file = new FileInfo("Resources/flag.xaml");
                XmlReader xmlReader = XmlReader.Create(file.FullName);
                Canvas userControl = (Canvas)XamlReader.Load(xmlReader);
                viewbox.Child = userControl;
                gameField.TopButton.Content = viewbox;
                gameField.IsFlagged = true;
                FlagsLeft--;
            }
            else
            {
                gameField.TopButton.Content = null;
                gameField.IsFlagged = false;
                FlagsLeft++;
            }
            OnPropertyChanged("FlagsLeft");
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            var time = (DateTime.Now - StartTime);
            if (time < TimeSpan.FromHours(1))
            {
                Time = time.ToString(@"mm\:ss");
            }
            else
            {
                Time = time.ToString(@"hh\:mm\:ss");
            }
            OnPropertyChanged("Time");
        }

        public void CheckForGameEnd()
        {
            if (GameFields.Where(x => x.IsMine == false && x.TopButton.IsVisible == true).Any())
            {
                return;
            }
            else
            {
                EndTheGame(true);
            }
        }

        public async void EndTheGame(bool gameWon)
        {
            DispatcherTimer.Stop();
            GameEndDialog gameEndDialog = new GameEndDialog();
            gameEndDialog.Owner = Application.Current.MainWindow;
            GameEndViewModel gameEndViewModel = new GameEndViewModel();
            if (gameWon)
            {
                gameEndViewModel.Title = "Congratulations!";
            }
            else
            {
                gameEndViewModel.Title = "Better luck next time!";
                await Task.Delay(2000);
            }
            gameEndViewModel.Time = "Your time: " + Time;
            gameEndDialog.DataContext = gameEndViewModel;
            if (gameEndDialog.ShowDialog() == true)
            {
                StartNewGame();
                CreateNewGameBoard();
            }
            else
            {
                Application.Current.MainWindow.Close();
            }
        }
    }
}
