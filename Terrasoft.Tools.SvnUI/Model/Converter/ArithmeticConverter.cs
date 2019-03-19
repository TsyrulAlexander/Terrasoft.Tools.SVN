using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Terrasoft.Tools.SvnUI.Model.Converter
{
	public class ArithmeticConverter : IValueConverter {
		private const string ArithmeticParseExpression = "([+\\-*/]{1,1})\\s{0,}(\\-?[\\d\\.]+)";
		private Regex arithmeticRegex = new Regex(ArithmeticParseExpression);

		#region IValueConverter Members

		object IValueConverter.Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
			if (value is double && parameter != null) {
				var param = parameter.ToString();
				if (param.Length > 0) {
					Match match = arithmeticRegex.Match(param);
					if (match.Groups.Count == 3) {
						string operation = match.Groups[1].Value.Trim();
						string numericValue = match.Groups[2].Value;
						if (double.TryParse(numericValue, out var number))
						{
							double valueAsDouble = (double) value;
							double returnValue = 0;

							switch (operation) {
								case "+":
									returnValue = valueAsDouble + number;
									break;

								case "-":
									returnValue = valueAsDouble - number;
									break;

								case "*":
									returnValue = valueAsDouble * number;
									break;

								case "/":
									returnValue = valueAsDouble / number;
									break;
							}
							return returnValue;
						}
					}
				}
			}
			return null;
		}

		object IValueConverter.ConvertBack(object value, Type targetType, object parameter,
			System.Globalization.CultureInfo culture) {
			throw new Exception("The method or operation is not implemented.");
		}

		#endregion
	}

}
