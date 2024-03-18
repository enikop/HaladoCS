using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace MazeApp.Helpers
{
    [ValueConversion(typeof(Color), typeof(Brush))]
    public class ColourToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                var colorValue = (Color)value;
                return new SolidColorBrush(colorValue);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Faulty argument, conversion failed."+ex.Message);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                var brushValue = (SolidColorBrush)value;
                return brushValue.Color;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Faulty argument, conversion failed." + ex.Message);
            }
        }
    }
}
