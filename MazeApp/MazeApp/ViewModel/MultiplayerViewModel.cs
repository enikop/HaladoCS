using MazeApp.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace MazeApp.ViewModel
{ 
    public class MultiplayerViewModel: INotifyPropertyChanged
    {
        private readonly Settings settings;
        private readonly Maze mazeAlt;
        private readonly Player playerOne;
        private readonly Player playerTwo;
        private int cellSize;

        private Timer aTimer;

        public int CellSize
        {
            get { return cellSize; }
            set
            {
                cellSize = value;
                NotifyPropertyChanged(nameof(CellSize));
                NotifyPropertyChanged(nameof(PlayerSize));
                NotifyPropertyChanged(nameof(MazeTotalHeight));
                NotifyPropertyChanged(nameof(MazeTotalWidth));
            }
        }

        public int LightRadius
        {
            get { return CellSize*5+CellSize/2; }
        }

        public int PlayerSize
        {
            get 
            {
                return CellSize - 10;
            }
        }

        public int MazeTotalWidth
        {
            get
            {
                var a = CellSize * MazeWidth;
                return CellSize * MazeWidth;
            }
        }

        public int MazeTotalHeight
        {
            get
            {
                return CellSize * MazeHeight;
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

        public GenerationAlgorithm Algorithm
        {
            get
            {
                return settings.Algorithm;
            }
        }

        public int MazeWidth
        {
            get
            {
                return settings.MazeWidth;
            }
        }

        public int MazeHeight
        {
            get
            {
                return settings.MazeHeight;
            }
        }

        public AppTheme CurrentTheme
        {
            get
            {
                return settings.CurrentTheme;
            }
        }

        public MultiplayerViewModel(Settings settings)
        {
            this.settings = settings;
            this.mazeAlt = new Maze(this.MazeWidth, this.MazeHeight, this.Algorithm);
            this.playerOne = new Player();
            this.playerTwo = new Player();

            this.CellSize = 40;

            SetTimer();

        }

        public Direction GetCellData(int row, int col)
        {
            return mazeAlt.GetCell(row, col);
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
            NotifyPropertyChanged(nameof(PlayerTwoX));
            NotifyPropertyChanged(nameof(PlayerTwoY));
        }

        public void MovePlayers()
        {
            Direction[] dirs = new Direction[] { Direction.North, Direction.South, Direction.West, Direction.East };
            foreach (Direction dir in dirs)
            {
                if (playerOne.MoveDirection.HasFlag(dir))
                {
                    (int, int) newPos = playerOne.PreviewStepOne(dir);
                    if(IsWithinBoundaries(newPos.Item1, newPos.Item2) && !mazeAlt.IsWallBetween((PlayerOneY, PlayerOneX), (newPos.Item2, newPos.Item1)))
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
                    if (IsWithinBoundaries(newPos.Item1, newPos.Item2) && !mazeAlt.IsWallBetween((PlayerTwoY, PlayerTwoX), (newPos.Item2, newPos.Item1)))
                    {
                        PlayerTwoX = newPos.Item1;
                        PlayerTwoY = newPos.Item2;
                        NotifyPropertyChanged(nameof(PlayerTwoX));
                        NotifyPropertyChanged(nameof(PlayerTwoY));
                    }
                }
            }
        }

        private bool IsWithinBoundaries(int x, int y)
        {
            if (x > -1 && x < MazeWidth && y > -1 && y < MazeHeight) return true;
            return false;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
