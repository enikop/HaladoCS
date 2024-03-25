using MazeApp.Model;
using MazeApp.Model.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Timers;
using System.Windows.Documents;

namespace MazeApp.ViewModel
{
    public class MultiplayerViewModel : GameViewModel
    {
        private readonly Player playerOne;
        private readonly Player playerTwo;

        private ObservableCollection<Position> pickups;

        public ObservableCollection<Position> Pickups
        {
            get { return pickups; }
        }

        public int PickupSize
        {
            get { return CellSize / 2; }
        }

        public int PlayerOneScore
        {
            get {
                return playerOne.Score;
            }
            set
            {
               playerOne.Score = value;
               NotifyPropertyChanged(nameof(PlayerOneScore));
            }
        }

        public int PlayerTwoScore
        {
            get
            {
                return playerTwo.Score;
            }
            set
            {
                playerTwo.Score = value;
                NotifyPropertyChanged(nameof(PlayerTwoScore));
            }
        }

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
            this.pickups = new();
            AddPickup();
            AddPickup();
            SetTimer();

        }

        private void AddPickup()
        {
            Random rand = new Random();
            int row, column;
            Position newPos;
            do
            {
                column = rand.Next(0, this.MazeWidth);
                row = rand.Next(0, this.MazeHeight);
                newPos = new Position(row, column);
            } while (pickups.Contains(newPos));
            pickups.Add(new Position(row, column));
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
