using Terrasoft.Tools.SvnUI.Properties;

namespace Terrasoft.Tools.SvnUI.Model.Property {
	public class StringProperty : BaseProperty<string> {

		public StringProperty(string caption, bool isRequired = false, object tag = null) : base(caption, isRequired,
			tag) {
		}

		public override bool IsValid(out string message) {
			if (IsRequired && string.IsNullOrWhiteSpace(Value)) {
				message = string.Format(Resources.IsEmpryPropertyValueMessage, Caption);
				return false;
			}
			return base.IsValid(out message);
		}
	}
}