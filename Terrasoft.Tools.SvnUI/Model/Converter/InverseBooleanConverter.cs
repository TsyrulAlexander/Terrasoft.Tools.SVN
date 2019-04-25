using System;
using System.Windows.Data;

namespace Terrasoft.Tools.SvnUI.Model.Converter {
	[ValueConversion(typeof(bool), typeof(bool))]
	public class InverseBooleanConverter : IValueConverter {

		public object Convert(object value, Type targetType, object parameter,
			System.Globalization.CultureInfo culture) {
			return value != null && !(bool) value;
		}

		public object ConvertBack(object value, Type targetType, object parameter,
			System.Globalization.CultureInfo culture) {
			throw new NotSupportedException();
		}
	}
}
