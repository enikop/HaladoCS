using System;
using System.Linq;
using System.Windows;

namespace MazeApp.Model.Enums
{
    public class ThemeManager
    {

        private static Uri GetThemeUri(Theme themeEnum)
        {
            Type type = typeof(Theme);
            ThemeUriAttribute? attr = (ThemeUriAttribute?)type.GetField(themeEnum.ToString())?.GetCustomAttributes(typeof(ThemeUriAttribute), false).SingleOrDefault();
            if (attr != null)
            {
                return new Uri(attr.Uri, UriKind.Relative);
            }
            else
            {
                throw new ArgumentException("No resource path specified for the given theme.");
            }
        }

        public static void ChangeTheme(Theme themeEnum)
        {
            Uri themeUri = GetThemeUri(themeEnum);
            ResourceDictionary theme = new ResourceDictionary() { Source = themeUri };
            Application.Current.Resources.MergedDictionaries.Clear();
            Application.Current.Resources.MergedDictionaries.Add(theme);
        }
    }
}
