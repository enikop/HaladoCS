using MazeApp.Model.Enums;
using System;
using System.Collections.Generic;

namespace MazeApp.Model
{
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
            GenerationStrategyManager.GetStrategy(generationAlgorithm).Generate(this);
        }

        public Direction GetCell(Position pos)
        {
            return cells[pos.Row, pos.Column];
        }

        public void InitializeMaze(bool isBlank = false)
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

        private Position? GetBottomNeighbour(Position position)
        {
            int neighbourRow = position.Row + 1;
            int neighbourCol = position.Column;
            if (neighbourRow < Height)
            {
                return new Position(neighbourRow, neighbourCol);
            }
            else
            {
                return null;
            }
        }

        private Position? GetTopNeighbour(Position position)
        {
            int neighbourRow = position.Row - 1;
            int neighbourCol = position.Column;
            if (neighbourRow >= 0)
            {
                return new Position(neighbourRow, neighbourCol);
            }
            else
            {
                return null;
            }
        }

        private Position? GetRightNeighbour(Position position)
        {
            int neighbourRow = position.Row;
            int neighbourCol = position.Column + 1;
            if (neighbourCol < Width)
            {
                return new Position(neighbourRow, neighbourCol);
            }
            else
            {
                return null;
            }
        }

        private Position? GetLeftNeighbour(Position position)
        {
            int neighbourRow = position.Row;
            int neighbourCol = position.Column - 1;
            if (neighbourCol >= 0)
            {
                return new Position(neighbourRow, neighbourCol);
            }
            else
            {
                return null;
            }
        }

        public List<Position> GetValidNeighbouringCells(Position position)
        {
            List<Position> neighbourCells = new List<Position>();
            var bottomNeighbour = GetBottomNeighbour(position);
            if (bottomNeighbour != null) { neighbourCells.Add(bottomNeighbour); }
            var rightNeighbour = GetRightNeighbour(position);
            if (rightNeighbour != null) { neighbourCells.Add(rightNeighbour); }
            var topNeighbour = GetTopNeighbour(position);
            if (topNeighbour != null) { neighbourCells.Add(topNeighbour); }
            var leftNeighbour = GetLeftNeighbour(position);
            if (leftNeighbour != null) { neighbourCells.Add(leftNeighbour); }
            return neighbourCells;
        }

        private (Direction, Direction)? GetCommonWallDirections(Position position1, Position position2)
        {
            if (position1.Row == position2.Row)
            {
                if (position1.Column == position2.Column + 1)
                {
                    return (Direction.West, Direction.East);
                }
                else if (position1.Column == position2.Column - 1)
                {
                    return (Direction.East, Direction.West);
                }
            }
            else if (position1.Column == position2.Column)
            {
                if (position1.Row == position2.Row + 1)
                {
                    return (Direction.North, Direction.South);
                }
                else if (position1.Row == position2.Row - 1)
                {
                    return (Direction.South, Direction.North);
                }
            }

            return null;
        }

        public void AddWallBetween(Position position1, Position position2)
        {
            (Direction, Direction)? commonSides = GetCommonWallDirections(position1, position2);
            if (commonSides.HasValue)
            {
                cells[position1.Row, position1.Column] |= commonSides.Value.Item1;
                cells[position2.Row, position2.Column] |= commonSides.Value.Item2;
            }
        }

        public void RemoveWallBetween(Position position1, Position position2)
        {
            (Direction, Direction)? commonSides = GetCommonWallDirections(position1, position2);
            if (commonSides.HasValue)
            {
                cells[position1.Row, position1.Column] &= ~commonSides.Value.Item1;
                cells[position2.Row, position2.Column] &= ~commonSides.Value.Item2;
            }
        }

        public bool IsWallBetween(Position position1, Position position2)
        {
            if (position1.Row < 0 || position1.Column < 0 || position2.Row < 0 || position2.Column < 0
                || position1.Row >= Height || position1.Column >= Width || position2.Row >= Height || position2.Column >= Width)
            {
                throw new ArgumentException("Invalid cell index.");
            }

            (Direction, Direction)? commonSides = GetCommonWallDirections(position1, position2);
            if (commonSides.HasValue)
            {
                return cells[position1.Row, position1.Column].HasFlag(commonSides.Value.Item1);
            }
            return false;
        }

        public void CopyWalls(Position targetPosition, Position sourcePosition)
        {
            cells[targetPosition.Row, targetPosition.Column] &= cells[sourcePosition.Row, sourcePosition.Column];
        }
    }
}
