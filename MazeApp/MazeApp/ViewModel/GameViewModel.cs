using MazeApp.Model;
using MazeApp.Model.Enums;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Timers;
using System.Windows.Threading;

namespace MazeApp.ViewModel
{
    public abstract class GameViewModel : INotifyPropertyChanged
    {
        private readonly Settings settings;
        protected readonly Maze maze;
        private int cellSize;
        private readonly int loopTime = 90;

        private Timer refreshTimer;

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
            get { return CellSize * 3; }
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

        public bool IsLimitedVisibility
        {
            get { return settings.IsLimitedVisibility; }
            set
            {
                settings.IsLimitedVisibility = value;
                NotifyPropertyChanged(nameof(IsLimitedVisibility));
            }
        }

        public string PlayerName
        {
            get
            {
                return settings.PlayerName;
            }
        }

        public GameViewModel(Settings settings)
        {
            this.settings = settings;
            this.maze = new Maze(this.MazeWidth, this.MazeHeight, this.Algorithm);
            //Cell size is proportionally changed with the largest dimension of the maze
            //It's 36 at a 16 grid length, but it does not grow beyond 60
            this.CellSize = (int)Math.Floor(Math.Min(60, 38 * 16.0 / Math.Max(this.MazeWidth, this.MazeHeight)));
            refreshTimer = new Timer(loopTime);
        }

        protected void SetTimer()
        {
            refreshTimer = new Timer(loopTime);
            refreshTimer.Elapsed += new ElapsedEventHandler( (s, e) => UpdatePlayerPositions());
            refreshTimer.AutoReset = true;
            refreshTimer.Enabled = true;
        }

        public void DisposeTimer()
        {
            if (refreshTimer != null)
            {
                refreshTimer.Stop();
                refreshTimer.Dispose();
            }
        }

        public abstract void UpdatePlayerPositions();

        protected bool IsWithinBoundaries(Position position)
        {
            if (position.Column > -1 && position.Column < MazeWidth && position.Row > -1 && position.Row < MazeHeight) return true;
            return false;
        }

        public Direction GetCellData(Position pos)
        {
            return maze.GetCell(pos);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
