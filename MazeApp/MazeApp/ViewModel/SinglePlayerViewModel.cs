using MazeApp.Model;
using MazeApp.Model.Enums;
using System;
using System.Threading;
using System.Timers;
using System.Windows;
using System.Windows.Threading;

namespace MazeApp.ViewModel
{
    public class SingleplayerViewModel: GameViewModel
    {
        private readonly Player playerOne;
        private Position prizePosition;
        private DispatcherTimer dispatcherTimer;
        private int gameTime;
        private bool isHighScore;

        public bool IsHighScore
        {
            get
            {
                return isHighScore;
            }
            set
            {
                isHighScore = value;
                NotifyPropertyChanged(nameof(IsHighScore));
            }
        }

        public int PrizeX {
            get
            {
                return prizePosition.Column;
            }
            set
            {
                prizePosition.Column = value;
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
                return prizePosition.Row;
            }
            set
            {
                prizePosition.Row = value;
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
            get { return playerOne.Position.Column; }
            set
            {
                playerOne.Position.Column = value;
                NotifyPropertyChanged(nameof(PlayerOneX));
            }
        }

        public int PlayerOneY
        {
            get { return playerOne.Position.Row; }
            set
            {
                playerOne.Position.Row = value;
                NotifyPropertyChanged(nameof(PlayerOneY));
            }
        }

        public int ElapsedTime { 
            get
            {
                return gameTime;
            }
            set
            {
                gameTime = value;
                NotifyPropertyChanged(nameof(ElapsedTime));
            }
        }

        public SingleplayerViewModel(Settings settings) : base(settings)
        {
            this.playerOne = new Player();
            this.prizePosition = new Position(0, 0);
            this.IsHighScore = false;
            ElapsedTime = 0;
            dispatcherTimer = new();
            dispatcherTimer.Interval = TimeSpan.FromSeconds(1);
            dispatcherTimer.Tick += new EventHandler((s,e) => increaseTimer());
            dispatcherTimer.Start();
            SetTimer();

        }

        private void increaseTimer()
        {
            ElapsedTime++;
        }

        public override void UpdatePlayerPositions()
        {
            MovePlayer();
        }

        public void MovePlayer()
        {
            Direction[] dirs = new Direction[] { Direction.North, Direction.South, Direction.West, Direction.East };
            foreach (Direction dir in dirs)
            {
                if (playerOne.MoveDirection.HasFlag(dir))
                {
                    Position newPos = playerOne.PreviewStepOne(dir);
                    if (IsWithinBoundaries(newPos) && !maze.IsWallBetween(playerOne.Position, newPos))
                    {
                        PlayerOneX = newPos.Column;
                        PlayerOneY = newPos.Row;
                    }
                    if (playerOne.Position.Equals(prizePosition))
                    {
                        dispatcherTimer.Stop();
                        if (IsNewHighScore())
                        {
                            IsHighScore = true;
                        }
                    }
                }
            }
        }

        private bool IsNewHighScore()
        {
            return ScoreLogger.LogScore("scores.json", 
                new Result(PlayerName, ElapsedTime, Algorithm, MazeWidth, MazeHeight, IsLimitedVisibility));
        }
    }
}
