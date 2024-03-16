using MazeApp.Model;
using MazeApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MazeApp.View
{
    /// <summary>
    /// Interaction logic for SingleplayerWindow.xaml
    /// </summary>
    public partial class SingleplayerWindow : Window
    {
        private readonly SingleplayerViewModel singleplayerViewModel;
        public SingleplayerWindow(Settings settings)
        {

            this.singleplayerViewModel = new SingleplayerViewModel(settings);

            DataContext = this.singleplayerViewModel;
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
            // Update player 1 key states
            if (key == Key.Up || key == Key.W)
                singleplayerViewModel.PlayerOneMoveDirection = pressed ? singleplayerViewModel.PlayerOneMoveDirection | Direction.North : singleplayerViewModel.PlayerOneMoveDirection & ~Direction.North;
            else if (key == Key.Down || key == Key.S)
                singleplayerViewModel.PlayerOneMoveDirection = pressed ? singleplayerViewModel.PlayerOneMoveDirection | Direction.South : singleplayerViewModel.PlayerOneMoveDirection & ~Direction.South;
            else if (key == Key.Left || key == Key.A)
                singleplayerViewModel.PlayerOneMoveDirection = pressed ? singleplayerViewModel.PlayerOneMoveDirection | Direction.West : singleplayerViewModel.PlayerOneMoveDirection & ~Direction.West;
            else if (key == Key.Right || key == Key.D)
                singleplayerViewModel.PlayerOneMoveDirection = pressed ? singleplayerViewModel.PlayerOneMoveDirection | Direction.East : singleplayerViewModel.PlayerOneMoveDirection & ~Direction.East;
        }

        private void DrawMaze()
        {
            for (int i = 0; i < singleplayerViewModel.MazeHeight; i++)
            {
                RowDefinition rowDef = new RowDefinition();
                rowDef.Height = new GridLength(singleplayerViewModel.CellSize, GridUnitType.Pixel);
                mainGrid.RowDefinitions.Add(rowDef);
                for (int j = 0; j < singleplayerViewModel.MazeWidth; j++)
                {
                    if (i == 0)
                    {
                        ColumnDefinition colDef = new ColumnDefinition();
                        colDef.Width = new GridLength(singleplayerViewModel.CellSize, GridUnitType.Pixel);
                        mainGrid.ColumnDefinitions.Add(colDef);
                    }
                    Canvas canvas = new Canvas();
                    canvas.Background = new SolidColorBrush(this.singleplayerViewModel.CurrentTheme.BackgroundColor);
                    canvas.Width = singleplayerViewModel.CellSize;
                    canvas.Height = singleplayerViewModel.CellSize;
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
            singleplayerViewModel.PlayerOneX = 0;
            singleplayerViewModel.PlayerOneY = 0;
            singleplayerViewModel.PrizeX = singleplayerViewModel.MazeWidth - 1;
            singleplayerViewModel.PrizeY = singleplayerViewModel.MazeHeight - 1;
        }

        private void AddToGrid(UIElement element, int row, int col)
        {
            Grid.SetRow(element, row);
            Grid.SetColumn(element, col);
            mainGrid.Children.Add(element);
        }


        private void DrawCell(Canvas canvas, int row, int col)
        {
            Direction sides = singleplayerViewModel.GetCellData(row, col);
            SolidColorBrush brush = new SolidColorBrush(singleplayerViewModel.CurrentTheme.MainForegroundColor);
            if (sides.HasFlag(Direction.North))
            {
                Line line = new Line
                {
                    X1 = 0,
                    Y1 = 0,
                    X2 = singleplayerViewModel.CellSize,
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
                    Y1 = singleplayerViewModel.CellSize,
                    X2 = singleplayerViewModel.CellSize,
                    Y2 = singleplayerViewModel.CellSize,
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
                    Y2 = singleplayerViewModel.CellSize,
                    Stroke = brush,
                    StrokeThickness = 2
                };
                canvas.Children.Add(line);
            }
            if (sides.HasFlag(Direction.East))
            {
                Line line = new Line
                {
                    X1 = singleplayerViewModel.CellSize,
                    Y1 = 0,
                    X2 = singleplayerViewModel.CellSize,
                    Y2 = singleplayerViewModel.CellSize,
                    Stroke = brush,
                    StrokeThickness = 2
                };
                canvas.Children.Add(line);
            }


        }

        protected override void OnClosed(EventArgs e)
        {
            singleplayerViewModel.DisposeTimer();
            base.OnClosed(e);

        }
    }
}
