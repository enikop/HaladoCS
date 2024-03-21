﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MazeApp.View
{
    public class RectangleConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length == 2 && values[0] is int width && values[1] is int height)
            {
                return new Rect(0, 0, width, height);
            }
            else if (values.Length == 5 && values[0] is int x && values[1] is int y && values[2] is int w && values[3] is int h && values[4] is int cellSize)
            {
                return new Rect(x*cellSize,y*cellSize,w,h);
            }
            return DependencyProperty.UnsetValue;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
