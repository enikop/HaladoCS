using System;

namespace MazeApp.Model
{
    public class RecursiveDivideStrategy : IGenerationStrategy
    {
        public void Generate(Maze maze)
        {
            maze.InitializeMaze(true);
            DoRecursiveDivide(maze, 0, 0, maze.Width, maze.Height);
        }

        private void DoRecursiveDivide(Maze maze, int x, int y, int width, int height)
        {
            if (width <= 1 || height <= 1)
            {
                return;
            }
            Random rand = new Random();

            //Divide with vertical and horizontal lines with 3 passages between the 4 areas
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
                    maze.AddWallBetween(new Position(i, x + widthDivide - 1), new Position(i, x + widthDivide));
                }
            }

            firstGate = (noPassageChoice == 2) ? -1 : x + rand.Next(widthDivide);
            secondGate = (noPassageChoice == 3) ? -1 : x + widthDivide + rand.Next(width - widthDivide);
            for (int i = x; i < x + width; i++)
            {
                if (i != firstGate && i != secondGate)
                {
                    maze.AddWallBetween(new Position(y + heightDivide - 1, i), new Position(y + heightDivide, i));
                }
            }

            //Recursion for the 4 divided areas
            DoRecursiveDivide(maze, x, y, widthDivide, heightDivide);
            DoRecursiveDivide(maze, x + widthDivide, y, width - widthDivide, heightDivide);
            DoRecursiveDivide(maze, x + widthDivide, y + heightDivide, width - widthDivide, height - heightDivide);
            DoRecursiveDivide(maze, x, y + heightDivide, widthDivide, height - heightDivide);
        }
    }
}
