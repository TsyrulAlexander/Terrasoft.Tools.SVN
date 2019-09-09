using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using Terrasoft.Configuration;
using Terrasoft.Core;
using Terrasoft.Tools.SvnUI.Model.Constant;
using Terrasoft.Tools.SvnUI.Model.Enums;
using Terrasoft.Tools.SvnUI.Model.EventArgs;
using Terrasoft.Tools.SvnUI.Model.File;
using Terrasoft.Tools.SvnUI.Properties;

namespace Terrasoft.Tools.SvnUI.ViewModel.Configuration
{
	public class ConfigurationViewModel : BaseViewModel {
		public IBrowserDialog BrowserDialog { get; }
		public ILogger Logger { get; }
		public ConfigurationOperation Operation { get; set; }
		public RelayCommand ExecuteCommand { get; set; }
		public RelayCommand<ConfigurationOperation> SetOperationCommand { get; set; }
		public ConfigurationViewModel(IBrowserDialog browserDialog, ILogger logger) {
			BrowserDialog = browserDialog;
			Logger = logger;
			ExecuteCommand = new RelayCommand(Execute);
			Messenger.Default.Register<StartConfigurationOperationEventArgs>(this, StartOperation);
			SetOperationCommand = new RelayCommand<ConfigurationOperation>(SetOperation);
		}
		public void SetOperation(ConfigurationOperation operation) {
			Operation = operation;
		}
		private void Execute() {
			Messenger.Default.Send(Operation);
		}
		private async void StartOperation(StartConfigurationOperationEventArgs eventArgs) {
			SetProgressState(true);
			var changeConfigurationRepository = new ChangeConfigurationRepository(Logger, 
				new ChangeConfigurationRepositoryParameter {
					PkgPath = eventArgs.Args[ConfigurationArgumentNameConstant.PkgPath],
					UrlPath = eventArgs.Args[ConfigurationArgumentNameConstant.UrlPath],
					SvnLogin = eventArgs.Args[ConfigurationArgumentNameConstant.UserLogin],
					SvnPassword = eventArgs.Args[ConfigurationArgumentNameConstant.UserPassword]
				}
			);
			await changeConfigurationRepository.RunAsync();
			SetProgressState(false);
			BrowserDialog.ShowInformationMessage(Resources.OperationComplite);
		}
	}
}
