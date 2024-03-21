using MazeApp.Model;
using MazeApp.Model.Enums;
using System;
using System.Timers;

namespace MazeApp.ViewModel
{
    public class MultiplayerViewModel: GameViewModel
    {
        private readonly Player playerOne;
        private readonly Player playerTwo;

        public Direction PlayerOneMoveDirection
        {
            get
            {
                return playerOne.MoveDirection;
            }
            set
            {
                playerOne.MoveDirection = value;
                NotifyPropertyChanged(nameof(PlayerOneMoveDirection));
            }
        }

        public Direction PlayerTwoMoveDirection
        {
            get
            {
                return playerTwo.MoveDirection;
            }
            set
            {
                playerTwo.MoveDirection = value;
                NotifyPropertyChanged(nameof(PlayerTwoMoveDirection));
            }
        }

        public int PlayerOneX
        {
            get { return playerOne.Position.Column; }
            set { 
                playerOne.Position.Column = value; 
                NotifyPropertyChanged(nameof(PlayerOneX));
            }
        }

        public int PlayerOneY
        {
            get { return playerOne.Position.Row; }
            set { 
                playerOne.Position.Row = value; 
                NotifyPropertyChanged(nameof(PlayerOneY));
            }
        }

        public int PlayerTwoX
        {
            get { return playerTwo.Position.Column; }
            set { 
                playerTwo.Position.Column = value; 
                NotifyPropertyChanged(nameof(PlayerTwoX));
            }
        }

        public int PlayerTwoY
        {
            get { return playerTwo.Position.Row; }
            set { 
                playerTwo.Position.Row = value;
                NotifyPropertyChanged(nameof(PlayerTwoY));
            }
        }

        public MultiplayerViewModel(Settings settings): base(settings)
        {
            this.playerOne = new Player();
            this.playerTwo = new Player();

            SetTimer();

        }

        public override void UpdatePlayerPositions() 
        {
            MovePlayers();
        }

        public void MovePlayers()
        {
            Direction[] dirs = new Direction[] { Direction.North, Direction.South, Direction.West, Direction.East };
            foreach (Direction dir in dirs)
            {
                if (playerOne.MoveDirection.HasFlag(dir))
                {
                    Position newPos = playerOne.PreviewStepOne(dir);
                    if(IsWithinBoundaries(newPos) && !maze.IsWallBetween(playerOne.Position, newPos))
                    {
                        PlayerOneX = newPos.Column;
                        PlayerOneY = newPos.Row;
                    }

                }
                if (playerTwo.MoveDirection.HasFlag(dir))
                {
                    Position newPos = playerTwo.PreviewStepOne(dir);
                    if (IsWithinBoundaries(newPos) && !maze.IsWallBetween(playerTwo.Position, newPos))
                    {
                        PlayerTwoX = newPos.Column;
                        PlayerTwoY = newPos.Row;
                    }
                }
            }
        }
    }
}
