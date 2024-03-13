using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    public class Settings: INotifyPropertyChanged
    {
        private List<AppTheme> themes;
        private (int, int) mazeDimensions;
        private GenerationAlgorithm algorithm;
        public GenerationAlgorithm Algorithm
        { 
            get 
            { 
                return algorithm;
            } set 
            { 
                if(value == GenerationAlgorithm.Tesselation)
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
                if (value > 1 && value <= 30)
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
                if (value > 1 && value <= 30)
                {
                    //If algorithm is tesselation, set both dimensions if value is the power of 2, otherwise return
                    if(this.Algorithm == GenerationAlgorithm.Tesselation && (value & (value - 1)) != 0)
                    {
                        return;
                    } else if (this.Algorithm == GenerationAlgorithm.Tesselation) 
                    {
                        mazeDimensions.Item1 = value;
                        NotifyPropertyChanged(nameof(MazeWidth));
                    }
                    mazeDimensions.Item2 = value;
                }
            }
        }

        private ColourTheme colourTheme;
        public ColourTheme ColourTheme {
            get
            {
                return colourTheme;
            }
            set
            {
                colourTheme = value;
                NotifyPropertyChanged(nameof(CurrentTheme));
            }
        }

        public AppTheme CurrentTheme
        {
            get
            {
                foreach(var theme in themes)
                {
                    if(this.ColourTheme == theme.ColourTheme)
                    {
                        return theme;
                    }
                }
                throw new Exception("Faulty theme definitions");
            }
        }

        public Settings(List<AppTheme> themes) 
        {
            this.Algorithm = GenerationAlgorithm.Wilson;
            this.ColourTheme = ColourTheme.Dark;
            this.MazeHeight = 16;
            this.MazeWidth = 16;
            this.themes = themes;
        }

        public Settings(Settings settings)
        {
            this.Algorithm = settings.Algorithm;
            this.ColourTheme = settings.ColourTheme;
            this.MazeHeight = settings.MazeHeight;
            this.MazeWidth = settings.MazeWidth;
            this.themes = settings.themes;
        }

        public void CopyFrom(Settings settings)
        {
            this.Algorithm = settings.Algorithm;
            this.ColourTheme = settings.ColourTheme;
            this.MazeHeight = settings.MazeHeight;
            this.MazeWidth = settings.MazeWidth;
            this.themes = settings.themes;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
