using MazeApp.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Timers;

namespace MazeApp.ViewModel
{ 
    public class MultiplayerViewModel: GameViewModel
    {
        private readonly Player playerOne;
        private readonly Player playerTwo;

        private Timer aTimer;

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
            get { return playerOne.X; }
            set { 
                playerOne.X = value; 
                NotifyPropertyChanged(nameof(PlayerOneX));
            }
        }

        public int PlayerOneY
        {
            get { return playerOne.Y; }
            set { 
                playerOne.Y = value; 
                NotifyPropertyChanged(nameof(PlayerOneY));
            }
        }

        public int PlayerTwoX
        {
            get { return playerTwo.X; }
            set { 
                playerTwo.X = value; 
                NotifyPropertyChanged(nameof(PlayerTwoX));
            }
        }

        public int PlayerTwoY
        {
            get { return playerTwo.Y; }
            set { 
                playerTwo.Y = value;
                NotifyPropertyChanged(nameof(PlayerTwoY));
            }
        }

        public MultiplayerViewModel(Settings settings): base(settings)
        {
            this.playerOne = new Player();
            this.playerTwo = new Player();

            SetTimer();

        }

        private void SetTimer()
        {
            aTimer = new Timer(120);
            aTimer.Elapsed += UpdatePlayerPositions;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }

        public void DisposeTimer()
        {
            if (aTimer != null)
            {
                aTimer.Stop();
                aTimer.Dispose();
            }
        }

        public void UpdatePlayerPositions(Object? source, ElapsedEventArgs elapsedEventArgs) 
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
                    (int, int) newPos = playerOne.PreviewStepOne(dir);
                    if(IsWithinBoundaries(newPos.Item1, newPos.Item2) && !maze.IsWallBetween((PlayerOneY, PlayerOneX), (newPos.Item2, newPos.Item1)))
                    {
                        PlayerOneX = newPos.Item1;
                        PlayerOneY = newPos.Item2;
                        NotifyPropertyChanged(nameof(PlayerOneX));
                        NotifyPropertyChanged(nameof(PlayerOneY));
                    }

                }
                if (playerTwo.MoveDirection.HasFlag(dir))
                {
                    (int, int) newPos = playerTwo.PreviewStepOne(dir);
                    if (IsWithinBoundaries(newPos.Item1, newPos.Item2) && !maze.IsWallBetween((PlayerTwoY, PlayerTwoX), (newPos.Item2, newPos.Item1)))
                    {
                        PlayerTwoX = newPos.Item1;
                        PlayerTwoY = newPos.Item2;
                        NotifyPropertyChanged(nameof(PlayerTwoX));
                        NotifyPropertyChanged(nameof(PlayerTwoY));
                    }
                }
            }
        }
    }
}
