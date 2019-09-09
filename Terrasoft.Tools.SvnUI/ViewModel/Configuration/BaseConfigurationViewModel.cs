using GalaSoft.MvvmLight.Messaging;
using Terrasoft.Tools.SvnUI.Model.Enums;
using Terrasoft.Tools.SvnUI.Model.EventArgs;
using Terrasoft.Tools.SvnUI.Model.File;

namespace Terrasoft.Tools.SvnUI.ViewModel.Configuration
{
	public abstract class BaseConfigurationViewModel : BaseViewModel {
		public IBrowserDialog BrowserDialog { get; }
		public BaseConfigurationViewModel(IBrowserDialog browserDialog) {
			BrowserDialog = browserDialog;
			Messenger.Default.Register<ConfigurationOperation>(this, OnRunConfigurationOperation);
		}
		protected virtual void OnRunConfigurationOperation(ConfigurationOperation operation) {
			if (operation != GetOperation()) {
				return;
			}
			if (!ValidateProperties(out string message)) {
				BrowserDialog.ShowInformationMessage(message);
				return;
			}
			StartOperation();
		}
		private void StartOperation() {
			var svnArguments = GetPropertiesToArguments();
			Messenger.Default.Send(new StartConfigurationOperationEventArgs {
				Operation = GetOperation(),
				Args = svnArguments
			});
		}
		protected abstract ConfigurationOperation GetOperation();
	}
}
