using System;
using System.Windows;

namespace MazeApp.Model.Enums
{
    public class ThemeHandler
    {
        public static void ChangeTheme(Uri themeUri)
        {
            ResourceDictionary theme = new ResourceDictionary() { Source = themeUri };
            Application.Current.Resources.MergedDictionaries.Clear();
            Application.Current.Resources.MergedDictionaries.Add(theme);
        }
    }
}
