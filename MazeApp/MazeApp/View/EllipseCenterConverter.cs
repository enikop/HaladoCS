using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MazeApp.View
{
    internal class EllipseCenterConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length == 3 && values[0] is int x && values[1] is int y && values[2] is int cellSize)
            {
                return new Point(x * cellSize + cellSize / 2.0, y * cellSize + cellSize / 2.0);
            }
            return DependencyProperty.UnsetValue;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
