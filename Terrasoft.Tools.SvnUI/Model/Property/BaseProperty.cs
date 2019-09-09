using System.ComponentModel;
using System.Runtime.CompilerServices;
using Terrasoft.Tools.SvnUI.Properties;

namespace Terrasoft.Tools.SvnUI.Model.Property {
	public abstract class BaseProperty : INotifyPropertyChanged {
		private bool _isRequired;
		private string _caption;
		private string _description;

		public bool IsRequired {
			get => _isRequired;
			set {
				_isRequired = value;
				OnPropertyChanged();
			}
		}

		public string Caption {
			get => _caption;
			set {
				_caption = value;
				OnPropertyChanged();
			}
		}

		public string Description {
			get => _description;
			set {
				_description = value;
				OnPropertyChanged();
			}
		}

		public object Tag { get; set; }

		public event PropertyChangedEventHandler PropertyChanged;

		public BaseProperty(string caption, bool isRequired = false, object tag = null) {
			Caption = caption;
			IsRequired = isRequired;
			Tag = tag;
		}

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		public virtual bool IsValid(out string message) {
			message = string.Empty;
			return true;
		}
	}
}
