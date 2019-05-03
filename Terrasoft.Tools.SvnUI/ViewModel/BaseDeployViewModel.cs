using System.Collections.Generic;
using GalaSoft.MvvmLight.Messaging;
using Terrasoft.Tools.SvnUI.Model.Enums;
using Terrasoft.Tools.SvnUI.Model.EventArgs;
using Terrasoft.Tools.SvnUI.Model.File;

namespace Terrasoft.Tools.SvnUI.ViewModel
{
    public abstract class BaseDeployViewModel : BaseViewModel
    {
        public BaseDeployViewModel(IBrowserDialog browserDialog) {
            BrowserDialog = browserDialog;
            Messenger.Default.Register<DeployOperation>(this, OnRunDeployOperation);
        }

        public IBrowserDialog BrowserDialog { get; }

        private void OnRunDeployOperation(DeployOperation operation) {
            if (operation != GetOperation()) {
                return;
            }

            if (!ValidateProperties(out string message)) {
                BrowserDialog.ShowInformationMessage(message);
                return;
            }

            StartDoployOperation();
        }

        protected virtual void StartDoployOperation() {
            Dictionary<string, string> arguments = GetPropertiesToArguments();
            Messenger.Default.Send(new StartDeployOperationEventArgs {Operation = GetOperation(), Args = arguments});
        }

        protected abstract DeployOperation GetOperation();
    }
}