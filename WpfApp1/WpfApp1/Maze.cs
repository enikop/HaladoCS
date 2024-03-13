using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows;
using static System.Net.Mime.MediaTypeNames;

namespace WpfApp1
{
    public enum EdgeType
    {
        Wall,
        Passage,
        NonConnected
    }
    public class Maze
    {
        //mapping from cell to index in matrix:
        //0 1 2 3
        //4 5 6 7...

        private EdgeType[,] neighbourMatrix;
        public int Width { get; }
        public int Height { get; }

        public Maze(int width, int height)
        {
            if (width <= 0 || height <= 0)
                throw new ArgumentException("Width and height must be positive integers.");

            Width = width;
            Height = height;

            // Initialize the neighbour matrix with all cells initially marked as non-connected
            neighbourMatrix = new EdgeType[Width * Height, Width * Height];
            //GenerateMazeRecursiveDivide(0,0, Width, Height);
            GenerateMazeWilson();
        }

        public EdgeType GetEdgeType(int i, int j)
        {
            if (i < 0 || j < 0 || i >= Width * Height || j >= Width * Height)
            {
                throw new ArgumentException("Invalid indices, the specified nodes do not exist.");
            }
            return neighbourMatrix[i, j];
        }

        private void InitializeMatrix(bool isBlank = false)
        {
            for (int i = 0; i < Width * Height; i++)
            {
                for (int j = i; j < Width * Height; j++)
                {
                    //Init non-connected, passages and walls
                    if (i == j)
                    {
                        neighbourMatrix[i, j] = EdgeType.Passage;
                    }
                    else if (GetNeighbouringNodeIndices(i).Contains(j))
                    {
                        neighbourMatrix[i, j] = isBlank ? EdgeType.Passage : EdgeType.Wall;
                        neighbourMatrix[j, i] = isBlank ? EdgeType.Passage : EdgeType.Wall;
                    }
                    else
                    {
                        neighbourMatrix[i, j] = EdgeType.NonConnected;
                        neighbourMatrix[j, i] = EdgeType.NonConnected;
                    }
                }
            }
        }

        public List<(Point, Point)> GetEdgeCoordinates(int cellSize)
        {
            List<(Point, Point)> edges = new();
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    int cellIndex = i * Width + j;
                    int? bottom = GetBottomNeighbourIndex(cellIndex);
                    int? right = GetRightNeighbourIndex(cellIndex);
                    if (bottom != null && GetEdgeType(cellIndex, (int)bottom) == EdgeType.Wall)
                    {
                        int x1 = j * cellSize;
                        int y1 = (i + 1) * cellSize;
                        int x2 = (j + 1) * cellSize;
                        int y2 = (i + 1) * cellSize;

                        edges.Add((new Point(x1, y1), new Point(x2, y2)));

                    }
                    if (right != null && GetEdgeType(cellIndex, (int)right) == EdgeType.Wall)
                    {
                        int x1 = (j + 1) * cellSize;
                        int y1 = i * cellSize;
                        int x2 = (j + 1) * cellSize;
                        int y2 = (i + 1) * cellSize;

                        edges.Add((new Point(x1, y1), new Point(x2, y2)));

                    }
                }
            }
            return edges;
        }


        private int? GetBottomNeighbourIndex(int index)
        {
            int bottomIndex = index + Width;
            if (bottomIndex < Width * Height) //if it's a valid index still
            {
                return bottomIndex;
            }
            else
            {
                return null;
            }
        }

        private int? GetRightNeighbourIndex(int index)
        {
            int rightIndex = index + 1;
            if (rightIndex / Width == index / Width && rightIndex < Width * Height) //if they are in the same line
            {
                return rightIndex;
            }
            else
            {
                return null;
            }
        }

        public List<int> GetNeighbouringNodeIndices(int index)
        {
            List<int> neighbourIndices = new List<int>();
            int leftIndex = index - 1;
            if (leftIndex / Width == index / Width && leftIndex >= 0) //if they are in the same line
            {
                neighbourIndices.Add(leftIndex);
            }
            int? rightIndex = GetRightNeighbourIndex(index);
            if (rightIndex != null) //if they are in the same line
            {
                neighbourIndices.Add((int)rightIndex);
            }
            int topIndex = index - Width;
            if (topIndex >= 0) //if it's a valid index still
            {
                neighbourIndices.Add(topIndex);
            }
            int? bottomIndex = GetBottomNeighbourIndex(index);
            if (bottomIndex != null) //if it's a valid index still
            {
                neighbourIndices.Add((int)bottomIndex);
            }
            return neighbourIndices;
        }

        private void GenerateMazeRecursiveDivide(int x, int y, int width, int height)
        {
            if (width == Width)
            {
                InitializeMatrix(true);
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
                    neighbourMatrix[(x + widthDivide - 1) + i * Width, (x + widthDivide) + i * Width] = EdgeType.Wall;
                    neighbourMatrix[(x + widthDivide) + i * Width, (x + widthDivide - 1) + i * Width] = EdgeType.Wall;
                }
            }

            firstGate = (noPassageChoice == 2) ? -1 : x + rand.Next(widthDivide);
            secondGate = (noPassageChoice == 3) ? -1 : x + widthDivide + rand.Next(width - widthDivide);
            for (int i = x; i < x + width; i++)
            {
                if (i != firstGate && i != secondGate)
                {
                    neighbourMatrix[i + (y + heightDivide - 1) * Width, i + (y + heightDivide) * Width] = EdgeType.Wall;
                    neighbourMatrix[i + (y + heightDivide) * Width, i + (y + heightDivide - 1) * Width] = EdgeType.Wall;
                }
            }

            //Recursion for the 4 divided areas
            GenerateMazeRecursiveDivide(x, y, widthDivide, heightDivide);
            GenerateMazeRecursiveDivide(x + widthDivide, y, width - widthDivide, heightDivide);
            GenerateMazeRecursiveDivide(x + widthDivide, y + heightDivide, width - widthDivide, height - heightDivide);
            GenerateMazeRecursiveDivide(x, y + heightDivide, widthDivide, height - heightDivide);
        }

        private void GenerateMazeTesselation(int size)
        {
            if (Width != Height) throw new InvalidOperationException("Cannot construct a non-square maze with this method.");
            if (size == Width)
            {
                InitializeMatrix();
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
                    int cellIndex = y1 * Width + x1;
                    List<int> neighbourIndices = GetNeighbouringNodeIndices(cellIndex);

                    foreach (var neighbourCellIndex in neighbourIndices)
                    {
                        int x2 = neighbourCellIndex % Width;
                        int y2 = neighbourCellIndex / Width;

                        //if the neighbour is not inside the copied area
                        if (x2 >= partSize || y2 >= partSize) continue;

                        neighbourMatrix[y1 * Width + x1 + partSize, y2 * Width + x2 + partSize] = neighbourMatrix[cellIndex, neighbourCellIndex];
                        neighbourMatrix[y2 * Width + x2 + partSize, y1 * Width + x1 + partSize] = neighbourMatrix[cellIndex, neighbourCellIndex];
                        neighbourMatrix[(y1 + partSize) * Width + x1, (y2 + partSize) * Width + x2] = neighbourMatrix[cellIndex, neighbourCellIndex];
                        neighbourMatrix[(y2 + partSize) * Width + x2, (y1 + partSize) * Width + x1] = neighbourMatrix[cellIndex, neighbourCellIndex];
                        neighbourMatrix[(y1 + partSize) * Width + x1 + partSize, (y2 + partSize) * Width + x2 + partSize] = neighbourMatrix[cellIndex, neighbourCellIndex];
                        neighbourMatrix[(y2 + partSize) * Width + x2 + partSize, (y1 + partSize) * Width + x1 + partSize] = neighbourMatrix[cellIndex, neighbourCellIndex];


                    }
                }
            }

            //Make 3 passages between the 4 parts
            List<Direction> choice = new List<Direction> { Direction.Up, Direction.Right, Direction.Down, Direction.Left }; //1:up, 2:right, 3:left, 4:down
            Random rand = new Random();
            int randIndex = rand.Next(choice.Count);
            choice.RemoveAt(randIndex);
            foreach (Direction direction in choice)
            {
                int x1, x2, y1, y2;
                if (direction == Direction.Up)
                {
                    x1 = partSize - 1;
                    x2 = partSize;
                    y1 = rand.Next(partSize);
                    y2 = y1;
                }
                else if (direction == Direction.Right)
                {
                    x1 = rand.Next(partSize) + partSize;
                    x2 = x1;
                    y1 = partSize - 1;
                    y2 = partSize;
                }
                else if (direction == Direction.Left)
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
                neighbourMatrix[x1 + y1 * Width, x2 + y2 * Width] = EdgeType.Passage;
                neighbourMatrix[x2 + y2 * Width, x1 + y1 * Width] = EdgeType.Passage;
            }
        }

        private enum Direction
        {
            Up, Down, Left, Right
        }

        public void GenerateMazeWilson()
        {
            InitializeMatrix();
            Random rand = new Random();
            List<int> maze = new List<int>();
            List<int> visited = new List<int>();
            List<int> toBeVisited = Enumerable.Range(0, Width * Height).ToList();

            int goal = rand.Next(Width * Height);
            toBeVisited.Remove(goal);
            maze.Add(goal);

            while (toBeVisited.Count > 0)
            {
                int current = toBeVisited[0];
                visited.Add(current);
                while (!maze.Contains(current))
                {
                    List<int> neighbours = GetNeighbouringNodeIndices(current);
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
                    neighbourMatrix[visited[i - 1], visited[i]] = EdgeType.Passage;
                    neighbourMatrix[visited[i], visited[i - 1]] = EdgeType.Passage;
                }
                visited.RemoveAt(visited.Count - 1);
                maze.AddRange(visited);
                toBeVisited = toBeVisited.Except(visited).ToList<int>();
                visited.Clear();
            }

        }

    }
}
