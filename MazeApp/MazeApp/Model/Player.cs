using MazeApp.Model.Enums;

namespace MazeApp.Model
{
    public class Player
    {

        public Direction MoveDirection { get; set; }
        public Position Position { get; set; }
        public int Score { get; set; }


        public Player()
        {
            this.Position = new Position(0, 0);
            this.Score = 0;
        }

        public Position PreviewStepOne(Direction dir)
        {
            int x = this.Position.Column;
            int y = this.Position.Row;
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
            return new Position(y, x);

        }
    }
}
