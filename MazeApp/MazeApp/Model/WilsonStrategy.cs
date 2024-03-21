using System;
using System.Collections.Generic;
using System.Linq;

namespace MazeApp.Model
{
    internal class WilsonStrategy : IGenerationStrategy
    {
        public void Generate(Maze maze)
        {
            maze.InitializeMaze();
            Random rand = new Random();
            List<Position> readyCells = new List<Position>();
            List<Position> visited = new List<Position>();
            List<Position> toBeVisited = Enumerable.Range(0, maze.Height)
                                                   .SelectMany(i => Enumerable.Range(0, maze.Width)
                                                   .Select(j => new Position(i, j))).ToList();

            Position goal = new Position(rand.Next(maze.Height), rand.Next(maze.Width));
            toBeVisited.Remove(goal);
            readyCells.Add(goal);

            while (toBeVisited.Count > 0)
            {
                Position current = toBeVisited[0];
                visited.Add(current);
                while (!readyCells.Contains(current))
                {
                    List<Position> neighbours = maze.GetValidNeighbouringCells(current);
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
                    maze.RemoveWallBetween(visited[i - 1], visited[i]);
                }
                visited.RemoveAt(visited.Count - 1);
                readyCells.AddRange(visited);
                toBeVisited = toBeVisited.Except(visited).ToList<Position>();
                visited.Clear();
            }
        }
    }
}
