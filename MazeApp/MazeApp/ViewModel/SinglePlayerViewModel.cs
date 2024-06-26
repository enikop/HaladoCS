﻿using MazeApp.Model;
using MazeApp.Model.Enums;
using System;
using System.Windows.Threading;

namespace MazeApp.ViewModel
{
    public class SingleplayerViewModel : GameViewModel
    {
        private readonly Player playerOne;
        private Position prizePosition;
        private DispatcherTimer dispatcherTimer;
        private int gameTime;
        private bool isHighScore;
        private bool hasGameEnded;
        private readonly string logPath = "scores.json";

        public Result? BestResult { get; set; }

        public bool IsBestResultValid
        {
            get
            {
                return this.BestResult != null;
            }
        }

        public bool HasGameEnded
        {
            get
            {
                return hasGameEnded;
            }
            set
            {
                hasGameEnded = value;
                NotifyPropertyChanged(nameof(HasGameEnded));
            }
        }

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

        public int PrizeX
        {
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
                return PlayerSize / 2;
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

        public int ElapsedTime
        {
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
            this.HasGameEnded = false;
            this.ElapsedTime = 0;
            this.BestResult = ScoreLogger.ReadBestResult(logPath, new Result(Algorithm, MazeWidth, MazeHeight, IsLimitedVisibility));
            dispatcherTimer = new();
            dispatcherTimer.Interval = TimeSpan.FromSeconds(1);
            dispatcherTimer.Tick += new EventHandler((s, e) => increaseTimer());
            dispatcherTimer.Start();
            SetTimer();

        }

        private void increaseTimer()
        {
            ElapsedTime++;
        }

        public override void UpdatePlayerPositions()
        {
            if (!hasGameEnded)
            {
                MovePlayer();
                if (playerOne.Position.Equals(prizePosition))
                {
                    HasGameEnded = true;
                    dispatcherTimer.Stop();
                    if (IsNewHighScore())
                    {
                        IsHighScore = true;

                    }
                }
            }

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
                }
            }
        }

        private bool IsNewHighScore()
        {
            return ScoreLogger.LogScore(logPath,
                new Result(PlayerName, ElapsedTime, Algorithm, MazeWidth, MazeHeight, IsLimitedVisibility));
        }
    }
}
