using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeApp.Model
{
    public enum ColourTheme
    {
        Dark,
        Light,
        Pink
    }

    public class ColourThemeHandler
    {
        public static Uri GetThemeUri(ColourTheme themeEnum)
        {
            string uri = "Themes/Dark.xaml";
            switch (themeEnum)
            {
                case ColourTheme.Dark:
                    uri = "Themes/Dark.xaml";
                    break;
                case ColourTheme.Light:
                    uri = "Themes/Light.xaml";
                    break;
                case ColourTheme.Pink:
                    uri = "Themes/Pink.xaml";
                    break;
            }
            return new Uri(uri, UriKind.Relative);
        }
    }
}
