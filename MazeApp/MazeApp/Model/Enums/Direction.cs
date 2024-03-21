using System;

namespace MazeApp.Model.Enums
{
    [Flags]
    public enum Direction
    {
        None = 1,
        North = 2,
        South = 4,
        East = 8,
        West = 16
    }
}
