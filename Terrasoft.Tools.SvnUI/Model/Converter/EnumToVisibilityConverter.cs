using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Terrasoft.Tools.SvnUI.Model.Converter
{
    public class EnumToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            if (value == null || parameter == null || !(value is Enum)) {
                return Visibility.Collapsed;
            }

            string currentState = value.ToString();
            string stateStrings = parameter.ToString();
            var found = false;
            foreach (string state in stateStrings.Split(',')) {
                found = currentState == state.Trim();
                if (found) {
                    break;
                }
            }

            return found ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}