using System;
using System.Windows;
using System.Windows.Data;

namespace Terrasoft.Tools.SvnUI.Model.Converter
{
	public class EnumBooleanConverter : IValueConverter {

		public object Convert(object value, Type targetType, object parameter,
			System.Globalization.CultureInfo culture) {
			if (!(parameter is string parameterString)) {
				return DependencyProperty.UnsetValue;
			}
			if (value == null || Enum.IsDefined(value.GetType(), value) == false) {
				return DependencyProperty.UnsetValue;
			}
			var parameterValue = Enum.Parse(value.GetType(), parameterString);
			return parameterValue.Equals(value);
		}

		public object ConvertBack(object value, Type targetType, object parameter,
			System.Globalization.CultureInfo culture) {
			if (!(parameter is string parameterString)) {
				return DependencyProperty.UnsetValue;
			}
			return Enum.Parse(targetType, parameterString);
		}
	}
}
