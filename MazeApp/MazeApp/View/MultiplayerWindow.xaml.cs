using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using MazeApp.ViewModel;
using MazeApp.Model;

namespace MazeApp.View
{
    /// <summary>
    /// Interaction logic for MultiplayerWindow.xaml
    /// </summary>
    public partial class MultiplayerWindow : Window
    { 

        private readonly MultiplayerViewModel multiplayerViewModel;
        public MultiplayerWindow(Settings settings)
        {

            this.multiplayerViewModel = new MultiplayerViewModel(settings);

            DataContext = this.multiplayerViewModel;
            InitializeComponent();
            this.PreviewKeyDown += Multiplayer_PreviewKeyDown;
            this.PreviewKeyUp += Multiplayer_PreviewKeyUp;

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

        public void UpdatePlayerKeyState(Key key, bool pressed)
        {
            // Update player 2 key states
            if (key == Key.W) 
                multiplayerViewModel.PlayerTwoMoveDirection = pressed ? multiplayerViewModel.PlayerTwoMoveDirection | Direction.North : multiplayerViewModel.PlayerTwoMoveDirection & ~Direction.North;
            else if (key == Key.S) 
                multiplayerViewModel.PlayerTwoMoveDirection = pressed ? multiplayerViewModel.PlayerTwoMoveDirection | Direction.South : multiplayerViewModel.PlayerTwoMoveDirection & ~Direction.South;
            else if (key == Key.A) 
                multiplayerViewModel.PlayerTwoMoveDirection = pressed ? multiplayerViewModel.PlayerTwoMoveDirection | Direction.West : multiplayerViewModel.PlayerTwoMoveDirection & ~Direction.West;
            else if (key == Key.D) 
                multiplayerViewModel.PlayerTwoMoveDirection = pressed ? multiplayerViewModel.PlayerTwoMoveDirection | Direction.East : multiplayerViewModel.PlayerTwoMoveDirection & ~Direction.East;

            // Update player 1 key states
            else if (key == Key.Up) 
                multiplayerViewModel.PlayerOneMoveDirection = pressed ? multiplayerViewModel.PlayerOneMoveDirection | Direction.North : multiplayerViewModel.PlayerOneMoveDirection & ~Direction.North;
            else if (key == Key.Down) 
                multiplayerViewModel.PlayerOneMoveDirection = pressed ? multiplayerViewModel.PlayerOneMoveDirection | Direction.South : multiplayerViewModel.PlayerOneMoveDirection & ~Direction.South;
            else if (key == Key.Left) 
                multiplayerViewModel.PlayerOneMoveDirection = pressed ? multiplayerViewModel.PlayerOneMoveDirection | Direction.West : multiplayerViewModel.PlayerOneMoveDirection & ~Direction.West;
            else if (key == Key.Right) 
                multiplayerViewModel.PlayerOneMoveDirection = pressed ? multiplayerViewModel.PlayerOneMoveDirection | Direction.East : multiplayerViewModel.PlayerOneMoveDirection & ~Direction.East;
        }

        private void DrawMaze()
        {
            for (int i = 0; i < multiplayerViewModel.MazeHeight; i++)
            {
                RowDefinition rowDef = new RowDefinition();
                rowDef.Height = new GridLength(multiplayerViewModel.CellSize, GridUnitType.Pixel);
                mainGrid.RowDefinitions.Add(rowDef);
                for (int j = 0; j < multiplayerViewModel.MazeWidth; j++)
                {
                    if (i == 0)
                    {
                        ColumnDefinition colDef = new ColumnDefinition();
                        colDef.Width = new GridLength(multiplayerViewModel.CellSize, GridUnitType.Pixel);
                        mainGrid.ColumnDefinitions.Add(colDef);
                    }
                    Canvas canvas = new Canvas();
                    canvas.Background = new SolidColorBrush(this.multiplayerViewModel.CurrentTheme.BackgroundColor);
                    canvas.Width = multiplayerViewModel.CellSize;
                    canvas.Height = multiplayerViewModel.CellSize;
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
            multiplayerViewModel.PlayerTwoX = 0;
            multiplayerViewModel.PlayerTwoY = 0;
            multiplayerViewModel.PlayerOneX = multiplayerViewModel.MazeWidth - 1;
            multiplayerViewModel.PlayerOneY = multiplayerViewModel.MazeHeight - 1;
        }

        private void AddToGrid(UIElement element, int row, int col)
        {
            Grid.SetRow(element, row);
            Grid.SetColumn(element, col);
            mainGrid.Children.Add(element);
        }


        private void DrawCell(Canvas canvas, int row, int col)
        {
            Direction sides = multiplayerViewModel.GetCellData(row, col);
            SolidColorBrush brush = new SolidColorBrush(multiplayerViewModel.CurrentTheme.MainForegroundColor);
            if (sides.HasFlag(Direction.North))
            {
                Line line = new Line
                {
                    X1 = 0,
                    Y1 = 0,
                    X2 = multiplayerViewModel.CellSize,
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
                    Y1 = multiplayerViewModel.CellSize,
                    X2 = multiplayerViewModel.CellSize,
                    Y2 = multiplayerViewModel.CellSize,
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
                    Y2 = multiplayerViewModel.CellSize,
                    Stroke = brush,
                    StrokeThickness = 2
                };
                canvas.Children.Add(line);
            }
            if (sides.HasFlag(Direction.East))
            {
                Line line = new Line
                {
                    X1 = multiplayerViewModel.CellSize,
                    Y1 = 0,
                    X2 = multiplayerViewModel.CellSize,
                    Y2 = multiplayerViewModel.CellSize,
                    Stroke = brush,
                    StrokeThickness = 2
                };
                canvas.Children.Add(line);
            }


        }

        protected override void OnClosed(EventArgs e)
        {
            multiplayerViewModel.DisposeTimer();
            base.OnClosed(e);

        }
    }
}
