using MazeApp.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace MazeApp.ViewModel
{
    public class GameViewModel : INotifyPropertyChanged
    {
        private readonly Settings settings;
        protected readonly Maze maze;
        private int cellSize;

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

        public AppTheme CurrentTheme
        {
            get
            {
                return settings.CurrentTheme;
            }
        }

        public GameViewModel(Settings settings)
        {
            this.settings = settings;
            this.maze = new Maze(this.MazeWidth, this.MazeHeight, this.Algorithm);

            this.CellSize = 40;


        }

        protected bool IsWithinBoundaries(int x, int y)
        {
            if (x > -1 && x < MazeWidth && y > -1 && y < MazeHeight) return true;
            return false;
        }

        public Direction GetCellData(int row, int col)
        {
            return maze.GetCell(row, col);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
