using GalaSoft.MvvmLight.Messaging;
using Terrasoft.Tools.SvnUI.Model.Enums;
using Terrasoft.Tools.SvnUI.Model.EventArgs;
using Terrasoft.Tools.SvnUI.Model.File;

namespace Terrasoft.Tools.SvnUI.ViewModel.Deploy {
	public abstract class BaseDeployViewModel : BaseViewModel {
		public IBrowserDialog BrowserDialog { get; }

		public BaseDeployViewModel(IBrowserDialog browserDialog) {
			BrowserDialog = browserDialog;
			Messenger.Default.Register<DeployOperation>(this, OnRunDeployOperation);
		}

		private void OnRunDeployOperation(DeployOperation operation) {
			if (operation != GetOperation()) {
				return;
			}
			if (!ValidateProperties(out string message)) {
				BrowserDialog.ShowInformationMessage(message);
				return;
			}
			StartDeployOperation();
		}

		protected virtual void StartDeployOperation() {
			var arguments = GetPropertiesToArguments();
			Messenger.Default.Send(new StartDeployOperationEventArgs {
				Operation = GetOperation(),
				Args = arguments
			});
		}

		protected abstract DeployOperation GetOperation();
	}
}
