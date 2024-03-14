using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace MazeApp.Model
{
    public enum GenerationAlgorithm
    {
        Tesselation, RecursiveDivide, Wilson
    }

    [Flags]
    public enum Direction
    {
        None = 1,
        North = 2,
        South = 4,
        East = 8,
        West = 16
    }
    public class Maze
    {
        private Direction[,] cells;
        public int Width { get; }
        public int Height { get; }


        public Maze(int width, int height, GenerationAlgorithm generationAlgorithm)
        {
            if (width <= 0 || height <= 0)
                throw new ArgumentException("Width and height must be positive integers.");

            Width = width;
            Height = height;

            cells = new Direction[height, width];
            switch (generationAlgorithm)
            {
                case GenerationAlgorithm.Tesselation:
                    GenerateMazeTesselation(Width);
                    break;
                case GenerationAlgorithm.RecursiveDivide:
                    GenerateMazeRecursiveDivide(0, 0, Width, Height);
                    break;
                case GenerationAlgorithm.Wilson:
                    GenerateMazeWilson();
                    break;
            }
        }

        public Direction GetCell(int row, int col)
        {
            return cells[row, col];
        }

        private void InitializeMaze(bool isBlank = false)
        {
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    if (!isBlank)
                    {
                        cells[i, j] = Direction.North | Direction.South | Direction.East | Direction.West;
                    }
                    else
                    {
                        //Inner cell
                        if (i != 0 && j != 0 && i != Height - 1 && j != Width - 1)
                        {
                            cells[i, j] = Direction.None;
                            continue;
                        }

                        //Horizontal borders
                        if (i == 0)
                        {
                            cells[i, j] = Direction.North;
                        }
                        else if (i == Height - 1)
                        {
                            cells[i, j] = Direction.South;
                        }

                        //Vertical borders
                        if (j == 0)
                        {
                            cells[i, j] |= Direction.West;
                        }
                        else if (j == Width - 1)
                        {
                            cells[i, j] |= Direction.East;
                        }
                    }
                }
            }
        }

        private (int, int)? GetBottomNeighbour(int row, int col)
        {
            int neighbourCol = col;
            int neighbourRow = row + 1;
            if (neighbourRow < Height)
            {
                return (neighbourRow, neighbourCol);
            }
            else
            {
                return null;
            }
        }

        private (int, int)? GetTopNeighbour(int row, int col)
        {
            int neighbourCol = col;
            int neighbourRow = row - 1;
            if (neighbourRow > -1)
            {
                return (neighbourRow, neighbourCol);
            }
            else
            {
                return null;
            }
        }

        private (int, int)? GetRightNeighbour(int row, int col)
        {
            int neighbourCol = col + 1;
            int neighbourRow = row;
            if (neighbourCol < Width)
            {
                return (neighbourRow, neighbourCol);
            }
            else
            {
                return null;
            }
        }
        private (int, int)? GetLeftNeighbour(int row, int col)
        {
            int neighbourCol = col - 1;
            int neighbourRow = row;
            if (neighbourCol > -1)
            {
                return (neighbourRow, neighbourCol);
            }
            else
            {
                return null;
            }
        }

        public List<(int, int)> GetValidNeighbouringCells(int row, int col)
        {
            List<(int, int)> neighbourCells = new();
            var bottomNeighbour = GetBottomNeighbour(row, col);
            if (bottomNeighbour != null) { neighbourCells.Add(((int, int))bottomNeighbour); }
            var rightNeighbour = GetRightNeighbour(row, col);
            if (rightNeighbour != null) { neighbourCells.Add(((int, int))rightNeighbour); }
            var topNeighbour = GetTopNeighbour(row, col);
            if (topNeighbour != null) { neighbourCells.Add(((int, int))topNeighbour); }
            var leftNeighbour = GetLeftNeighbour(row, col);
            if (leftNeighbour != null) { neighbourCells.Add(((int, int))leftNeighbour); }
            return neighbourCells;

        }


        private (Direction, Direction)? GetCommonWallDirections((int, int) cell1, (int, int) cell2)
        {
            if (cell1.Item1 == cell2.Item1)
            {
                if (cell1.Item2 == cell2.Item2 + 1)
                {
                    return (Direction.West, Direction.East);
                }
                else if (cell1.Item2 == cell2.Item2 - 1)
                {
                    return (Direction.East, Direction.West);
                }
            }
            else if (cell1.Item2 == cell2.Item2)
            {
                if (cell1.Item1 == cell2.Item1 + 1)
                {
                    return (Direction.North, Direction.South);
                }
                else if (cell1.Item1 == cell2.Item1 - 1)
                {
                    return (Direction.South, Direction.North);
                }
            }

            return null;
        }

        private void AddWallBetween((int, int) cell1, (int, int) cell2)
        {
            (Direction, Direction)? commonSides = GetCommonWallDirections(cell1, cell2);
            if (commonSides.HasValue)
            {
                cells[cell1.Item1, cell1.Item2] |= (((Direction, Direction))commonSides).Item1;
                cells[cell2.Item1, cell2.Item2] |= (((Direction, Direction))commonSides).Item2;
            }
        }

        private void RemoveWallBetween((int, int) cell1, (int, int) cell2)
        {
            (Direction, Direction)? commonSides = GetCommonWallDirections(cell1, cell2);
            if (commonSides.HasValue)
            {
                cells[cell1.Item1, cell1.Item2] &= ~(((Direction, Direction))commonSides).Item1;
                cells[cell2.Item1, cell2.Item2] &= ~(((Direction, Direction))commonSides).Item2;
            }
        }

        public bool IsWallBetween((int, int) cell1, (int, int) cell2) //(row, col)
        {
            //If a cell index is invalid, throw Exception
            if (cell1.Item1 < 0 || cell1.Item2 < 0 || cell2.Item1 < 0 || cell2.Item2 < 0
                || cell1.Item1 >= Height || cell1.Item2 >= Width || cell2.Item1 >= Height || cell2.Item2 >= Width) throw new ArgumentException("Invalid cell index.");
            (Direction, Direction)? commonSides = GetCommonWallDirections(cell1, cell2);
            if (commonSides.HasValue)
            {
                if (cells[cell1.Item1, cell1.Item2].HasFlag((((Direction, Direction))commonSides).Item1))
                {
                    return true;
                }
            }
            return false;
        }

        private void GenerateMazeTesselation(int size)
        {
            if (Width != Height || (Width & (Width - 1)) != 0) throw new InvalidOperationException("Cannot construct a non-square or non 2 pow maze with this method.");
            if (size == Width)
            {
                InitializeMaze();
            }
            if (size <= 1)
            {
                return;
            }

            //Make the smaller maze
            int partSize = size / 2;
            GenerateMazeTesselation(partSize);

            //Create 4 identical parts of the smaller maze next to each other
            for (int x1 = 0; x1 < partSize; x1++)
            {
                for (int y1 = 0; y1 < partSize; y1++)
                {
                    cells[y1, x1 + partSize] &= cells[y1, x1];
                    cells[y1 + partSize, x1 + partSize] &= cells[y1, x1];
                    cells[y1 + partSize, x1] &= cells[y1, x1];
                }
            }
            List<Direction> choice = new List<Direction> { Direction.North, Direction.East, Direction.South, Direction.West }; //1:up, 2:right, 3:left, 4:down
            Random rand = new Random();
            int randIndex = rand.Next(choice.Count);
            choice.RemoveAt(randIndex);
            foreach (Direction direction in choice)
            {
                int x1, x2, y1, y2;
                if (direction == Direction.North)
                {
                    x1 = partSize - 1;
                    x2 = partSize;
                    y1 = rand.Next(partSize);
                    y2 = y1;
                }
                else if (direction == Direction.East)
                {
                    x1 = rand.Next(partSize) + partSize;
                    x2 = x1;
                    y1 = partSize - 1;
                    y2 = partSize;
                }
                else if (direction == Direction.West)
                {
                    x1 = rand.Next(partSize);
                    x2 = x1;
                    y1 = partSize - 1;
                    y2 = partSize;
                }
                else
                {
                    x1 = partSize - 1;
                    x2 = partSize;
                    y1 = rand.Next(partSize) + partSize;
                    y2 = y1;
                }
                RemoveWallBetween((y1, x1), (y2, x2));
            }
        }

        private void GenerateMazeRecursiveDivide(int x, int y, int width, int height)
        {
            if (width == Width)
            {
                InitializeMaze(true);
            }
            if (width <= 1 || height <= 1)
            {
                return;
            }
            Random rand = new Random();

            //Divider vertical and horizontal lines with 3 passages between the areas
            int widthDivide = rand.Next(width - 1) + 1;
            int heightDivide = rand.Next(height - 1) + 1;

            //Choose the gate that won't be drawn (3 passages are enough between 4 areas)
            int noPassageChoice = rand.Next(4);

            int firstGate = (noPassageChoice == 0) ? -1 : y + rand.Next(heightDivide);
            int secondGate = (noPassageChoice == 1) ? -1 : y + heightDivide + rand.Next(height - heightDivide);
            for (int i = y; i < y + height; i++)
            {
                if (i != firstGate && i != secondGate)
                {
                    AddWallBetween((i, x + widthDivide - 1), (i, x + widthDivide));
                    //cells[i, x + widthDivide - 1] |= Direction.East;
                    //cells[i, x + widthDivide] |= Direction.West;
                }
            }

            firstGate = (noPassageChoice == 2) ? -1 : x + rand.Next(widthDivide);
            secondGate = (noPassageChoice == 3) ? -1 : x + widthDivide + rand.Next(width - widthDivide);
            for (int i = x; i < x + width; i++)
            {
                if (i != firstGate && i != secondGate)
                {
                    AddWallBetween((y + heightDivide - 1, i), (y + heightDivide, i));
                    //cells[y + heightDivide - 1, i] |= Direction.South;
                    //cells[y + heightDivide, i] |= Direction.North;
                }
            }

            //Recursion for the 4 divided areas
            GenerateMazeRecursiveDivide(x, y, widthDivide, heightDivide);
            GenerateMazeRecursiveDivide(x + widthDivide, y, width - widthDivide, heightDivide);
            GenerateMazeRecursiveDivide(x + widthDivide, y + heightDivide, width - widthDivide, height - heightDivide);
            GenerateMazeRecursiveDivide(x, y + heightDivide, widthDivide, height - heightDivide);
        }

        public void GenerateMazeWilson()
        {
            InitializeMaze();
            Random rand = new Random();
            List<(int, int)> maze = new();
            List<(int, int)> visited = new();
            List<(int, int)> toBeVisited = Enumerable.Range(0, Height).SelectMany(i => Enumerable.Range(0, Width).Select(j => (i, j))).ToList();

            (int, int) goal = (rand.Next(Width), rand.Next(Height));
            toBeVisited.Remove(goal);
            maze.Add(goal);

            while (toBeVisited.Count > 0)
            {
                (int, int) current = toBeVisited[0];
                visited.Add(current);
                while (!maze.Contains(current))
                {
                    List<(int, int)> neighbours = GetValidNeighbouringCells(current.Item1, current.Item2);
                    current = neighbours[rand.Next(neighbours.Count)];
                    int index = visited.IndexOf(current);
                    if (index != -1)
                    {
                        visited.RemoveRange(index, visited.Count - index);
                    }
                    visited.Add(current);
                }
                for (int i = 1; i < visited.Count; i++)
                {
                    RemoveWallBetween(visited[i - 1], visited[i]);
                }
                visited.RemoveAt(visited.Count - 1);
                maze.AddRange(visited);
                toBeVisited = toBeVisited.Except(visited).ToList<(int, int)>();
                visited.Clear();
            }
        }
    }
}
