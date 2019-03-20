namespace Terrasoft.Tools.SvnUI.Model.Property {
	public abstract class BaseProperty<T> : BaseProperty {

		private T _value;
		public T Value {
			get => _value;
			set {
				_value = value;
				OnPropertyChanged();
			}
		}

		protected BaseProperty(string caption, bool isRequired = false, object tag = null) : base(caption, isRequired,
			tag) {
		}

		public override string ToString() {
			return Value.ToString();
		}
	}
}