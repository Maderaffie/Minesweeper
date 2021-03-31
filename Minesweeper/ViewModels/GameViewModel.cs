using Minesweeper.Commands;
using Minesweeper.Models;
using Minesweeper.Services;
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
        public BaseCommand AddBoardGridCommand { get; set; }
        public BaseCommand PauseGameCommand { get; set; }
        public BaseCommand RestartGameCommand { get; set; }
        public GameButtonCommand GameButtonCommand { get; set; }
        public GameButtonRightClickCommand GameButtonRightClickCommand { get; set; }
        public GameButtonLeftDoubleClickCommand GameButtonLeftDoubleClickCommand { get; set; }

        public ObservableCollection<Grid> BoardGrid { get; set; }
        public List<GameField> GameFields { get; set; }

        private int _flagsLeft;
        public int FlagsLeft
        {
            get { return _flagsLeft; }
            set
            {
                _flagsLeft = value;
                OnPropertyChanged(nameof(FlagsLeft));
            }
        }
        private string _time;
        public string Time
        {
            get { return _time; }
            set
            {
                _time = value;
                OnPropertyChanged(nameof(Time));
            }
        }

        private readonly MainWindowViewModel _mainWindowViewModel;
        private readonly TimerService timerService;
        private bool gameStarted;
        private bool gameEnded;
        private int _numberOfRows;
        private int _numberOfColumns;
        private int _numberOfMines;

        public GameViewModel(MainWindowViewModel mainWindowViewModel, int rows, int columns, int mines)
        {
            _mainWindowViewModel = mainWindowViewModel;

            timerService = new TimerService();
            timerService.DispatcherTimer.Tick += DispatcherTimer_Tick;

            GameButtonCommand = new GameButtonCommand(this);
            GameButtonRightClickCommand = new GameButtonRightClickCommand(this);
            GameButtonLeftDoubleClickCommand = new GameButtonLeftDoubleClickCommand(this);
            PauseGameCommand = new BaseCommand(PauseTheGame);
            RestartGameCommand = new BaseCommand(RestartTheGame);
            AddBoardGridCommand = new BaseCommand(OpenNewGameDialog);

            StartNewGame(rows, columns, mines);
        }

        public void StartNewGame(int rows, int columns, int mines)
        {
            gameStarted = false;
            gameEnded = false;
            Time = "00:00";
            timerService.ResetTimer();
            GameFields = new List<GameField>();
            CreateNewGameBoard(rows, columns, mines);
        }

        public void CreateNewGameBoard(int rows, int columns, int mines)
        {
            Grid grid = new Grid();
            _numberOfRows = rows;
            _numberOfColumns = columns;
            _numberOfMines = mines;

            grid.Height = _numberOfRows * 100;
            grid.Width = _numberOfColumns * 100;
            FlagsLeft = _numberOfMines;

            Application.Current.MainWindow.MinWidth = _numberOfColumns * 50 / 0.8;
            Application.Current.MainWindow.MinHeight = _numberOfRows * 50 / 0.9;

            for (int i = 0; i < _numberOfRows; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition());
            }

            for (int i = 0; i < _numberOfColumns; i++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            for (int i = 0; i < _numberOfRows; i++)
            {
                for (int j = 0; j < _numberOfColumns; j++)
                {
                    Button bottomButton = CreateBottomButton();
                    Button topButton = CreateTopButton();

                    GameFields.Add(new GameField(topButton, bottomButton, i, j));
                    Grid.SetColumn(bottomButton, j);
                    Grid.SetRow(bottomButton, i);
                    grid.Children.Add(bottomButton);

                    Grid.SetColumn(topButton, j);
                    Grid.SetRow(topButton, i);
                    grid.Children.Add(topButton);
                }
            }

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

        public Button CreateBottomButton()
        {
            Button bottomButton = new Button
            {
                Style = Application.Current.FindResource("BottomButton") as Style
            };

            MouseGesture mouseGesture = new MouseGesture
            {
                MouseAction = MouseAction.LeftDoubleClick
            };

            MouseBinding mouseBinding = new MouseBinding
            {
                Gesture = mouseGesture,
                Command = GameButtonLeftDoubleClickCommand,
                CommandParameter = bottomButton
            };

            bottomButton.InputBindings.Add(mouseBinding);
            return bottomButton;
        }

        public Button CreateTopButton()
        {
            Button topButton = new Button
            {
                Command = GameButtonCommand,
                Style = Application.Current.FindResource("TopButton") as Style
            };
            topButton.CommandParameter = topButton;

            MouseGesture mouseGesture = new MouseGesture
            {
                MouseAction = MouseAction.RightClick
            };

            MouseBinding mouseBinding = new MouseBinding
            {
                Gesture = mouseGesture,
                Command = GameButtonRightClickCommand,
                CommandParameter = topButton
            };

            topButton.InputBindings.Add(mouseBinding);
            return topButton;
        }

        public void HandleGameFieldClick(GameField gameField)
        {
            if (gameEnded)
            {
                return;
            }
            if (!gameStarted)
            {
                gameStarted = true;
                GenerateMines(gameField);
                timerService.StartTimer();
            }
            bool canPlayerContinue = ShowField(gameField);
            if (canPlayerContinue)
            {
                CheckForGameEnd();
            }
            else
            {
                RevealAllMines(gameField);
                EndTheGame(false);
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
            else
            {
                CheckForGameEnd();
            }
        }

        public void GenerateMines(GameField gameField)
        {
            int minesPlaced = 0;
            var fieldsAroundClickedField = GetFieldsAround(gameField);
            List<GameField> fieldsWithoutMine = GameFields.Where(x => !fieldsAroundClickedField.Contains(x) &&
                                                                 x != gameField).ToList();

            while (minesPlaced < _numberOfMines)
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

        public void AddButtonContent(GameField gameField, string filePath)
        {
            Viewbox viewbox = new Viewbox();
            viewbox.Margin = new Thickness(5);
            gameField.TopButton.Visibility = Visibility.Hidden;
            if (filePath == "")
            {
                return;
            }

            FileInfo file = new FileInfo(filePath);
            XmlReader xmlReader = XmlReader.Create(file.FullName);
            Canvas userControl = (Canvas)XamlReader.Load(xmlReader);
            viewbox.Child = userControl;
            gameField.BottomButton.Content = viewbox;
        }

        public bool ShowField(GameField gameField)
        {
            if (gameField.IsFlagged)
            {
                return true;
            }

            if (gameField.IsMine)
            {
                AddButtonContent(gameField, "Resources/mine.xaml");
                return false;
            }

            if (gameField.NumberOfMinesAround != 0)
            {
                AddButtonContent(gameField, "Resources/" + gameField.NumberOfMinesAround + ".xaml");
                return true;
            }
            else
            {
                AddButtonContent(gameField, "");
                var fieldsAround = GetFieldsAround(gameField);
                fieldsAround = fieldsAround.Where(x => x.TopButton.IsVisible == true).ToList();
                foreach (var field in fieldsAround)
                {
                    ShowField(field);
                }
                return true;
            }
        }

        public void RevealAllMines(GameField gameField)
        {
            gameEnded = true;
            var listOfMines = GameFields.Where(x => x.IsMine).ToList();
            listOfMines.Remove(gameField);
            foreach (var mine in listOfMines)
            {
                if (mine.IsFlagged)
                {
                    AddButtonContent(mine, "Resources/mine_win.xaml");
                }
                else
                {
                    AddButtonContent(mine, "Resources/mine.xaml");
                }
            }
        }

        public void SetUpTheFlag(GameField gameField)
        {
            if (!gameStarted || gameEnded)
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
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            Time = timerService.Tick();
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
            timerService.StopTimer();
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
                StartNewGame(_numberOfRows, _numberOfColumns, _numberOfMines);
            }
            else
            {
                _mainWindowViewModel.SetMenuViewModel();
            }
        }

        public void OpenNewGameDialog()
        {
            timerService.StopTimer();
            var newGameViewModel = _mainWindowViewModel.OpenNewGameDialog();
            if (newGameViewModel != null)
            {
                StartNewGame(newGameViewModel.NumberOfRows, newGameViewModel.NumberOfColumns, newGameViewModel.NumberOfMines);
            }
            else
            {
                timerService.StartTimer();
            }
        }

        public void PauseTheGame()
        {
            timerService.StopTimer();
            PauseViewModel pauseViewModel = new PauseViewModel();
            PauseDialog pauseDialog = new PauseDialog
            {
                Owner = Application.Current.MainWindow,
                DataContext = pauseViewModel
            };
            if (pauseDialog.ShowDialog() == true)
            {
                _mainWindowViewModel.SetMenuViewModel();
            }
            else
            {
                if (gameStarted && !gameEnded)
                {
                    timerService.StartTimer();
                }
            }
        }

        public void RestartTheGame()
        {
            StartNewGame(_numberOfRows, _numberOfColumns, _numberOfMines);
        }
    }
}
