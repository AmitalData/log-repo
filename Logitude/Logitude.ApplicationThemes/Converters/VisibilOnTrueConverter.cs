using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Logitude.ApplicationThemes.Converters
{
    public class VisibilOnTrueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Visibility show = Visibility.Visible;
            Visibility hide = Visibility.Collapsed;
            if (value != null)
            {
                bool newValue = (bool)value;
                if (parameter != null)
                {
                    newValue = !newValue;
                }
                if (!newValue)
                {
                    return hide;
                }
                else return show;
            }
            else
            {
                return hide;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return 0;
        }
    }
}
