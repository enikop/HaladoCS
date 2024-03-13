using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MazeApp.Model
{
    public class AppTheme
    {
        public ColourTheme ColourTheme { get; }
        public Color BackgroundColor { get; set; }
        public Color MainForegroundColor { get; set; }
        public Color SecondaryForegroundColor { get; set; }
        public Color TertiaryForegroundColor { get; set; }
        public Color PlayerOneColor { get; set; }
        public Color PlayerTwoColor { get; set; }

        public Brush BackgroundBrush
        {
            get
            {
                return new SolidColorBrush(BackgroundColor);
            }
        }

        public Brush MainForegroundBrush
        {
            get
            {
                return new SolidColorBrush(MainForegroundColor);
            }
        }

        public Brush SecondaryForegroundBrush
        {
            get
            {
                return new SolidColorBrush(SecondaryForegroundColor);
            }
        }

        public Brush TertiaryForegroundBrush
        {
            get
            {
                return new SolidColorBrush(TertiaryForegroundColor);
            }
        }
        public Brush PlayerOneBrush
        {
            get
            {
                return new SolidColorBrush(PlayerOneColor);
            }
        }
        public Brush PlayerTwoBrush
        {
            get
            {
                return new SolidColorBrush(PlayerTwoColor);
            }
        }


        public AppTheme(ColourTheme colourTheme, Color backgroundColor, Color mainForegroundColor, Color secondaryForegroundColor, Color tertiaryForegroundColor, Color playerOneColor, Color playerTwoColor)
        {
            ColourTheme = colourTheme;
            BackgroundColor = backgroundColor;
            MainForegroundColor = mainForegroundColor;
            SecondaryForegroundColor = secondaryForegroundColor;
            TertiaryForegroundColor = tertiaryForegroundColor;
            PlayerOneColor = playerOneColor;
            PlayerTwoColor = playerTwoColor;
        }
    }
}
