using MazeApp.Model;
using MazeApp.Model.Enums;
using MazeApp.ViewModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MazeApp.View
{
    public class GameAssistant
    {
        public static Direction GetUpdatedDirection(Direction toUpdate, Direction changedDirection, bool isAdded)
        {
            return isAdded ? toUpdate | changedDirection : toUpdate & ~changedDirection;
        }

        public static void DrawMaze(Grid mainGrid, GameViewModel gameViewModel, Brush foregroundBrush)
        {
            for (int i = 0; i < gameViewModel.MazeHeight; i++)
            {
                RowDefinition rowDef = new RowDefinition();
                rowDef.Height = new GridLength(gameViewModel.CellSize, GridUnitType.Pixel);
                mainGrid.RowDefinitions.Add(rowDef);
                for (int j = 0; j < gameViewModel.MazeWidth; j++)
                {
                    if (i == 0)
                    {
                        ColumnDefinition colDef = new ColumnDefinition();
                        colDef.Width = new GridLength(gameViewModel.CellSize, GridUnitType.Pixel);
                        mainGrid.ColumnDefinitions.Add(colDef);
                    }
                    Canvas canvas = new Canvas();
                    canvas.Background = new SolidColorBrush(Colors.Transparent);
                    canvas.Width = gameViewModel.CellSize;
                    canvas.Height = gameViewModel.CellSize;
                    DrawCell(canvas, i, j, gameViewModel, foregroundBrush);
                    AddToGrid(mainGrid, canvas, i, j);
                }
                ColumnDefinition columnDefinition = new ColumnDefinition();
                columnDefinition.Width = new GridLength(1, GridUnitType.Star);
                mainGrid.ColumnDefinitions.Add(columnDefinition);
            }
            RowDefinition rowDefinition = new RowDefinition();
            rowDefinition.Height = new GridLength(1, GridUnitType.Star);
            mainGrid.RowDefinitions.Add(rowDefinition);
        }

        private static void AddToGrid(Grid mainGrid, UIElement element, int row, int col)
        {
            Grid.SetRow(element, row);
            Grid.SetColumn(element, col);
            mainGrid.Children.Add(element);
        }


        private static void DrawCell(Canvas canvas, int row, int col, GameViewModel gameViewModel, Brush brush)
        {
            Direction sides = gameViewModel.GetCellData(new Position(row, col));
            if (sides.HasFlag(Direction.North))
            {
                Line line = new Line
                {
                    X1 = 0,
                    Y1 = 0,
                    X2 = gameViewModel.CellSize,
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
                    Y1 = gameViewModel.CellSize,
                    X2 = gameViewModel.CellSize,
                    Y2 = gameViewModel.CellSize,
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
                    Y2 = gameViewModel.CellSize,
                    Stroke = brush,
                    StrokeThickness = 2
                };
                canvas.Children.Add(line);
            }
            if (sides.HasFlag(Direction.East))
            {
                Line line = new Line
                {
                    X1 = gameViewModel.CellSize,
                    Y1 = 0,
                    X2 = gameViewModel.CellSize,
                    Y2 = gameViewModel.CellSize,
                    Stroke = brush,
                    StrokeThickness = 2
                };
                canvas.Children.Add(line);
            }


        }
    }
}
