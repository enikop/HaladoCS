using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using MazeApp.Model.Enums;

namespace MazeApp.Model
{
    public class Settings: INotifyPropertyChanged
    {
        private string playerName;
        private (int, int) mazeDimensions;
        private GenerationAlgorithm algorithm;
        private bool isLimitedVisibility;

        public string PlayerName
        {
            get 
            { 
                return playerName; 
            }
            set 
            { 
                if(value.Length > 0)
                {
                    playerName = value;
                    NotifyPropertyChanged(nameof(PlayerName));
                }
            }
        }

        public bool IsLimitedVisibility { 
            get
            {
                return isLimitedVisibility;
            }
            set
            {
                isLimitedVisibility = value;
                NotifyPropertyChanged(nameof(IsLimitedVisibility));
            }
        }

        public GenerationAlgorithm Algorithm
        {
            get
            {
                return algorithm;
            }
            set
            {
                if (value == GenerationAlgorithm.Tesselation)
                {
                    MazeWidth = 16;
                    MazeHeight = 16;
                    NotifyPropertyChanged(nameof(MazeWidth));
                    NotifyPropertyChanged(nameof(MazeHeight));
                }
                this.algorithm = value;
            }
        }
        public int MazeWidth
        {
            get
            {
                return mazeDimensions.Item1;
            }
            set
            {
                if (value > 3 && value <= 20)
                {
                    //If algorithm is tesselation, set both dimensions if value is the power of 2, otherwise return
                    if (this.Algorithm == GenerationAlgorithm.Tesselation && (value & (value - 1)) != 0)
                    {
                        return;
                    }
                    else if (this.Algorithm == GenerationAlgorithm.Tesselation)
                    {
                        mazeDimensions.Item2 = value;
                        NotifyPropertyChanged(nameof(MazeHeight));
                    }
                    mazeDimensions.Item1 = value;
                }

            }
        }
        public int MazeHeight
        {
            get
            {
                return mazeDimensions.Item2;
            }
            set
            {
                if (value > 3 && value <= 20)
                {
                    //If algorithm is tesselation, set both dimensions if value is the power of 2, otherwise return
                    if (this.Algorithm == GenerationAlgorithm.Tesselation && (value & (value - 1)) != 0)
                    {
                        return;
                    }
                    else if (this.Algorithm == GenerationAlgorithm.Tesselation)
                    {
                        mazeDimensions.Item1 = value;
                        NotifyPropertyChanged(nameof(MazeWidth));
                    }
                    mazeDimensions.Item2 = value;
                }
            }
        }
        public Theme ColourTheme { get; set; }

        public Settings()
        {
            this.Algorithm = GenerationAlgorithm.Wilson;
            this.ColourTheme = Theme.Dark;
            this.MazeHeight = 6;
            this.MazeWidth = 6;
            this.IsLimitedVisibility = false;
            this.playerName = "Unknown Owl";
        }

        public Settings(Settings settings)
        {
            this.Algorithm = settings.Algorithm;
            this.ColourTheme = settings.ColourTheme;
            this.MazeHeight = settings.MazeHeight;
            this.MazeWidth = settings.MazeWidth;
            this.IsLimitedVisibility = settings.IsLimitedVisibility;
            this.playerName = settings.PlayerName;
        }

        public void CopyFrom(Settings settings)
        {
            this.Algorithm = settings.Algorithm;
            this.ColourTheme = settings.ColourTheme;
            this.MazeHeight = settings.MazeHeight;
            this.MazeWidth = settings.MazeWidth;
            this.IsLimitedVisibility = settings.IsLimitedVisibility;
            this.PlayerName = settings.PlayerName;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
