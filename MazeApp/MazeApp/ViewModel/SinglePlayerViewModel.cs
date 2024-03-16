using MazeApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace MazeApp.ViewModel
{
    public class SingleplayerViewModel: GameViewModel
    {
        private readonly Player playerOne;
        private Timer aTimer;
        private (int, int) prizePosition;

        public int PrizeX {
            get
            {
                return prizePosition.Item1;
            }
            set
            {
                prizePosition.Item1 = value;
                NotifyPropertyChanged(nameof(PrizeX));
            }
        }

        public int PrizeSize
        {
            get
            {
                return PlayerSize / 3;
            }
        }
        public int PrizeY
        {
            get
            {
                return prizePosition.Item2;
            }
            set
            {
                prizePosition.Item2 = value;
                NotifyPropertyChanged(nameof(PrizeY));
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

        public int PlayerOneX
        {
            get { return playerOne.X; }
            set
            {
                playerOne.X = value;
                NotifyPropertyChanged(nameof(PlayerOneX));
            }
        }

        public int PlayerOneY
        {
            get { return playerOne.Y; }
            set
            {
                playerOne.Y = value;
                NotifyPropertyChanged(nameof(PlayerOneY));
            }
        }

        public SingleplayerViewModel(Settings settings) : base(settings)
        {
            this.playerOne = new Player();
            this.PrizeX = 0;
            this.PrizeY = 0;
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
                    if (IsWithinBoundaries(newPos.Item1, newPos.Item2) && !maze.IsWallBetween((PlayerOneY, PlayerOneX), (newPos.Item2, newPos.Item1)))
                    {
                        PlayerOneX = newPos.Item1;
                        PlayerOneY = newPos.Item2;
                        NotifyPropertyChanged(nameof(PlayerOneX));
                        NotifyPropertyChanged(nameof(PlayerOneY));
                    }

                }
            }
        }
    }
}
