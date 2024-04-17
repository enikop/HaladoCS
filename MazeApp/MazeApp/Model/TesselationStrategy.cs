using MazeApp.Model.Enums;
using System;
using System.Collections.Generic;

namespace MazeApp.Model
{
    public class TesselationStrategy : IGenerationStrategy
    {
        public void Generate(Maze maze)
        {
            if (maze.Width != maze.Height || (maze.Width & (maze.Width - 1)) != 0) throw new InvalidOperationException("Can only construct a square maze that's sides are powers of 2 with this method.");
            maze.InitializeMaze();
            DoTesselation(maze, maze.Width);
        }

        private void DoTesselation(Maze maze, int size)
        {
            if (size <= 1)
            {
                return;
            }

            //Make the smaller maze
            int partSize = size / 2;
            DoTesselation(maze, partSize);

            //Create 4 identical parts of the smaller maze next to each other
            for (int x1 = 0; x1 < partSize; x1++)
            {
                for (int y1 = 0; y1 < partSize; y1++)
                {
                    Position sourcePos = new Position(y1, x1);
                    maze.CopyWalls(new Position(y1, x1 + partSize), sourcePos);
                    maze.CopyWalls(new Position(y1 + partSize, x1 + partSize), sourcePos);
                    maze.CopyWalls(new Position(y1 + partSize, x1), sourcePos);
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
                maze.RemoveWallBetween(new Position(y1, x1), new Position(y2, x2));
            }
        }
    }
}
