using System.Threading.Tasks;
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
		private ConfigurationOperation _operation;
		public IBrowserDialog BrowserDialog { get; }
		public ILogger Logger { get; }
		public ConfigurationOperation Operation {
			get => _operation;
			set {
				_operation = value;
				RaisePropertyChanged();
			}
		}
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
			if (eventArgs.Operation == ConfigurationOperation.ChangeRepository) {
				await ChangeConfigurationRepository(eventArgs);
			} else if (eventArgs.Operation == ConfigurationOperation.PackageCompression) {
				await CompressionProjects(eventArgs);
			}
			SetProgressState(false);
			BrowserDialog.ShowInformationMessage(Resources.OperationComplite);
		}
		private async Task CompressionProjects(StartConfigurationOperationEventArgs eventArgs) {
			var pkgPath = eventArgs.Args[ConfigurationArgumentNameConstant.PkgPath];
			var savePath = eventArgs.Args[ConfigurationArgumentNameConstant.SavePath];
			PackageCompressionHelper compressionHelper = new PackageCompressionHelper(Logger);
			await compressionHelper.CompressionProjectsAsync(pkgPath, savePath);
		}
		private async Task ChangeConfigurationRepository(StartConfigurationOperationEventArgs eventArgs) {
			var changeConfigurationRepository = new ChangeConfigurationRepository(Logger,
				new ChangeConfigurationRepositoryParameter {
					PkgPath = eventArgs.Args[ConfigurationArgumentNameConstant.PkgPath],
					UrlPath = eventArgs.Args[ConfigurationArgumentNameConstant.UrlPath],
					SvnLogin = eventArgs.Args[ConfigurationArgumentNameConstant.UserLogin],
					SvnPassword = eventArgs.Args[ConfigurationArgumentNameConstant.UserPassword]
				});
			await changeConfigurationRepository.RunAsync();
		}
	}
}
