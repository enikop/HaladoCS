namespace MazeApp.Model
{
    public class Pickup
    {
        private Position pos;
        public int Column
        {
            get
            {
                return pos.Column;
            }
            set
            {
                pos.Column = value;
                IsToMove = false;
            }
        }
        public int Row
        {
            get
            {
                return pos.Row;
            }
            set
            {
                pos.Row = value;
                IsToMove = false;
            }
        }

        public Position Position
        {
            get
            {
                return pos;
            }
            set
            {
                pos = value;
                IsToMove = false;
            }
        }
        public bool IsToMove { get; set; }

        public Pickup()
        {
            this.pos = new Position(0, 0);
            IsToMove = false;
        }
    }
}
