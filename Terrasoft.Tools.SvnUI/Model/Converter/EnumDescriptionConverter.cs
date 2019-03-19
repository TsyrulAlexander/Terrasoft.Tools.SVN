using System;
using System.ComponentModel;
using System.Windows.Data;

namespace Terrasoft.Tools.SvnUI.Model.Converter
{
	public class EnumDescriptionConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
			if (value == null && targetType != null) {
				return string.Empty;
			}
			return GetDescription((Enum)value);
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
			if (value == null || string.IsNullOrWhiteSpace(value.ToString())) {
				return null;
			}
			return Enum.Parse(targetType, value.ToString());
		}

		public static string GetDescription(Enum en) {
			var type = en.GetType();
			var memInfo = type.GetMember(en.ToString());
			if (memInfo.Length > 0) {
				object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
				if (attrs.Length > 0) {
					return ((DescriptionAttribute)attrs[0]).Description;
				}
			}
			return en.ToString();
		}
	}
}
