using System.Collections.Generic;
using GalaSoft.MvvmLight;
using Terrasoft.Tools.SvnUI.Model.Property;

namespace Terrasoft.Tools.SvnUI.ViewModel
{
	public abstract class BaseViewModel : ViewModelBase {
		protected virtual bool ValidateProperties(out string message) {
			var properties = GetProperties();
			foreach (var property in properties) {
				if (!property.IsValid(out message)) {
					return false;
				}
			}
			message = string.Empty;
			return true;
		}

		protected virtual Dictionary<string, string> GetPropertiesToArguments() {
			var args = new Dictionary<string, string>();
			var properties = GetProperties();
			foreach (var property in properties) {
				var argument = GetPropertyToArgument(property);
				args.Add(argument.name, argument.value);
			}
			return args;
		}

		protected virtual (string name, string value) GetPropertyToArgument(BaseProperty property) {
			var operationKey = (string)property.Tag;
			return (operationKey.ToLower(), property.ToString());
		}

		protected virtual IEnumerable<BaseProperty> GetProperties() {
			return new BaseProperty[0];
		}
	}
}
