using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MazeApp.ViewModel;
using MazeApp.Model;
using System.Timers;

namespace MazeApp.View
{
    /// <summary>
    /// Interaction logic for MultiplayerWindow.xaml
    /// </summary>
    public partial class MultiplayerWindow : Window
    { 
        private Maze mazeAlt;
        private int cellSize = 40;
        private Ellipse player1;
        private Ellipse player2;

        private Direction player1Dir;
        private Direction player2Dir;
        private System.Timers.Timer aTimer;

        private readonly MultiplayerViewModel multiplayerViewModel;
        public MultiplayerWindow(Settings settings)
        {
            InitializeComponent();

            this.multiplayerViewModel = new MultiplayerViewModel(settings);

            DataContext = this.multiplayerViewModel;

            mazeAlt = new Maze(multiplayerViewModel.MazeWidth, multiplayerViewModel.MazeHeight, multiplayerViewModel.Algorithm);
            player1 = new Ellipse { Width = cellSize - 10, Height = cellSize - 10, Fill = new SolidColorBrush(multiplayerViewModel.CurrentTheme.PlayerOneColor) };
            player2 = new Ellipse { Width = cellSize - 10, Height = cellSize - 10, Fill = new SolidColorBrush(multiplayerViewModel.CurrentTheme.PlayerTwoColor) };
            this.PreviewKeyDown += Multiplayer_PreviewKeyDown;
            this.PreviewKeyUp += Multiplayer_PreviewKeyUp;

            SetTimer();
            DrawMaze();
        }

        private void Multiplayer_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            UpdatePlayerKeyState(e.Key, true);
        }

        private void Multiplayer_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            UpdatePlayerKeyState(e.Key, false);
        }

        private void SetTimer()
        {
            // Timer with 0.09 sec interval
            aTimer = new System.Timers.Timer(90);
            aTimer.Elapsed += UpdatePlayerPositions;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }

        public void UpdatePlayerKeyState(Key key, bool pressed)
        {
            // Update player 2 key states
            if (key == Key.W) player2Dir = pressed ? player2Dir | Direction.North : player2Dir & ~Direction.North;
            else if (key == Key.S) player2Dir = pressed ? player2Dir | Direction.South : player2Dir & ~Direction.South;
            else if (key == Key.A) player2Dir = pressed ? player2Dir | Direction.West : player2Dir & ~Direction.West;
            else if (key == Key.D) player2Dir = pressed ? player2Dir | Direction.East : player2Dir & ~Direction.East;

            // Update player 1 key states
            else if (key == Key.Up) player1Dir = pressed ? player1Dir | Direction.North : player1Dir & ~Direction.North;
            else if (key == Key.Down) player1Dir = pressed ? player1Dir | Direction.South : player1Dir & ~Direction.South;
            else if (key == Key.Left) player1Dir = pressed ? player1Dir | Direction.West : player1Dir & ~Direction.West;
            else if (key == Key.Right) player1Dir = pressed ? player1Dir | Direction.East : player1Dir & ~Direction.East;
        }

        private void UpdatePlayerPositions(Object? source, ElapsedEventArgs e)
        {
            Direction[] dirs = new Direction[] { Direction.North, Direction.South, Direction.West, Direction.East };
            foreach (Direction dir in dirs)
            {
                if (player1Dir.HasFlag(dir))
                {
                    MovePlayer(player1, dir);
                }
            }
            foreach (Direction dir in dirs)
            {
                if (player2Dir.HasFlag(dir))
                {
                    MovePlayer(player2, dir);
                }
            }
        }

        private void MovePlayerOperations(Ellipse player, Direction dir)
        {
            int playerRow = Grid.GetRow(player);
            int playerCol = Grid.GetColumn(player);

            int pressedRow = playerRow;
            int pressedCol = playerCol;

            switch (dir)
            {
                case Direction.North:
                    pressedRow -= 1;
                    break;
                case Direction.South:
                    pressedRow += 1;
                    break;
                case Direction.East:
                    pressedCol += 1;
                    break;
                case Direction.West:
                    pressedCol -= 1;
                    break;
                default:
                    return;

            }

            if ((Math.Abs(pressedRow - playerRow) == 1 && pressedCol == playerCol) || (Math.Abs(pressedCol - playerCol) == 1 && pressedRow == playerRow))
            {
                try
                {
                    if (!mazeAlt.IsWallBetween((pressedRow, pressedCol), (playerRow, playerCol)))
                    {
                        mainGrid.Children.Remove(player);
                        AddToGrid(player, pressedRow, pressedCol);
                    }
                }
                catch (Exception ex)
                {
                    return;
                }
            }
        }

        private void MovePlayer(Ellipse player, Direction dir)
        {
            Dispatcher.Invoke(() =>
            {
                MovePlayerOperations(player, dir);
            });

        }

        private void DrawMaze()
        {
            for (int i = 0; i < mazeAlt.Height; i++)
            {
                RowDefinition rowDef = new RowDefinition();
                rowDef.Height = new GridLength(cellSize, GridUnitType.Pixel);
                mainGrid.RowDefinitions.Add(rowDef);
                for (int j = 0; j < mazeAlt.Width; j++)
                {
                    if (i == 0)
                    {
                        ColumnDefinition colDef = new ColumnDefinition();
                        colDef.Width = new GridLength(cellSize, GridUnitType.Pixel);
                        mainGrid.ColumnDefinitions.Add(colDef);
                    }
                    Canvas canvas = new Canvas();
                    canvas.Background = new SolidColorBrush(this.multiplayerViewModel.CurrentTheme.BackgroundColor);
                    canvas.Width = cellSize;
                    canvas.Height = cellSize;
                    DrawCell(canvas, i, j);
                    AddToGrid(canvas, i, j);
                }
                ColumnDefinition columnDefinition = new ColumnDefinition();
                columnDefinition.Width = new GridLength(1, GridUnitType.Star);
                mainGrid.ColumnDefinitions.Add(columnDefinition);
            }
            RowDefinition rowDefinition = new RowDefinition();
            rowDefinition.Height = new GridLength(1, GridUnitType.Star);
            mainGrid.RowDefinitions.Add(rowDefinition);
            AddToGrid(player2, 0, 0);
            AddToGrid(player1, mazeAlt.Height - 1, mazeAlt.Width - 1);
        }

        private void AddToGrid(UIElement element, int row, int col)
        {
            Grid.SetRow(element, row);
            Grid.SetColumn(element, col);
            mainGrid.Children.Add(element);
        }


        private void DrawCell(Canvas canvas, int row, int col)
        {
            Direction sides = mazeAlt.GetCell(row, col);
            SolidColorBrush brush = new SolidColorBrush(multiplayerViewModel.CurrentTheme.MainForegroundColor);
            if (sides.HasFlag(Direction.North))
            {
                Line line = new Line
                {
                    X1 = 0,
                    Y1 = 0,
                    X2 = cellSize,
                    Y2 = 0,
                    Stroke = brush,
                    StrokeThickness = 2
                };
                canvas.Children.Add(line);
            }
            if (sides.HasFlag(Direction.South))
            {
                Line line = new Line
                {
                    X1 = 0,
                    Y1 = cellSize,
                    X2 = cellSize,
                    Y2 = cellSize,
                    Stroke = brush,
                    StrokeThickness = 2
                };
                canvas.Children.Add(line);
            }
            if (sides.HasFlag(Direction.West))
            {
                Line line = new Line
                {
                    X1 = 0,
                    Y1 = 0,
                    X2 = 0,
                    Y2 = cellSize,
                    Stroke = brush,
                    StrokeThickness = 2
                };
                canvas.Children.Add(line);
            }
            if (sides.HasFlag(Direction.East))
            {
                Line line = new Line
                {
                    X1 = cellSize,
                    Y1 = 0,
                    X2 = cellSize,
                    Y2 = cellSize,
                    Stroke = brush,
                    StrokeThickness = 2
                };
                canvas.Children.Add(line);
            }


        }

        protected override void OnClosed(EventArgs e)
        {
            aTimer.Stop();
            aTimer.Dispose();
            base.OnClosed(e);

        }
    }
}
