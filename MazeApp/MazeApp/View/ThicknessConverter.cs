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
    public class ThicknessConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length == 5 && values[0] is int x && values[1] is int y && values[2] is int width && values[3] is int height && values[4] is int cellSize)
            {
                double marginLeft = cellSize * x - width / 2.0 + cellSize / 2.0;
                double marginTop = cellSize * y - height / 2.0 + cellSize / 2.0;
                return new Thickness(marginLeft > 0 ? marginLeft : 0, marginTop > 0 ? marginTop : 0, 0, 0);
            }
            return DependencyProperty.UnsetValue;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
