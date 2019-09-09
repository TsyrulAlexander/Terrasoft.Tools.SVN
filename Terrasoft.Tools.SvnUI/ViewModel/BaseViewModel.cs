using System.Collections.Generic;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using Terrasoft.Tools.SvnUI.Model.EventArgs;
using Terrasoft.Tools.SvnUI.Model.Property;

namespace Terrasoft.Tools.SvnUI.ViewModel
{
	public abstract class BaseViewModel : ViewModelBase {
		private bool _inProgress;
		public bool InProgress {
			get => _inProgress;
			set {
				_inProgress = value;
				RaisePropertyChanged();
			}
		}

		public BaseViewModel() {
			Messenger.Default.Register<ProgressEventArgs>(this, OnProgressChange);
		}
		protected virtual void OnProgressChange(ProgressEventArgs progress) {
			DispatcherHelper.CheckBeginInvokeOnUI(() => InProgress = progress.InProgress);
		}
		protected virtual void SetProgressState(bool state) {
			Messenger.Default.Send(new ProgressEventArgs {
				InProgress = state,
				Owner = this
			});
		}

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
