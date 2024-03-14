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
