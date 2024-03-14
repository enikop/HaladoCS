using MazeApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MazeApp.Model
{
    public class Player
    {

        public Direction MoveDirection { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public (int, int) PreviewStepOne(Direction dir)
        {
            int x = this.X;
            int y = this.Y;
            switch (dir)
            {
                case Direction.North:
                    y -= 1;
                    break;
                case Direction.South:
                    y += 1;
                    break;
                case Direction.East:
                    x += 1;
                    break;
                case Direction.West:
                    x -= 1;
                    break;

            }
            return (x, y);

        }
    }
}
