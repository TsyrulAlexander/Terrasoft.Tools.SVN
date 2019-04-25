using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using Terrasoft.Core;
using Terrasoft.Core.DeployApp.Database;
using Terrasoft.Core.DeployApp.Database.MsSql;
using Terrasoft.Core.DeployApp.Database.Oracle;
using Terrasoft.Tools.SvnUI.Model.Constant;
using Terrasoft.Tools.SvnUI.Model.Enums;
using Terrasoft.Tools.SvnUI.Model.EventArgs;
using Terrasoft.Tools.SvnUI.Model.File;

namespace Terrasoft.Tools.SvnUI.ViewModel
{
	public class DeployAppViewModel : ViewModelBase {
		public IBrowserDialog BrowserDialog { get; }
		private bool _inProgress;
		private DeployOperation _operation = DeployOperation.RestoreDb;
		public bool InProgress {
			get => _inProgress;
			set {
				_inProgress = value;
				RaisePropertyChanged();
			}
		}
		public DeployOperation Operation {
			get => _operation;
			set {
				_operation = value;
				RaisePropertyChanged();
			}
		}
		public RelayCommand ExecuteCommand { get; set; }
		public RelayCommand<DeployOperation> SetDeployOperationCommand { get; set; }
		public DeployAppViewModel(IBrowserDialog browserDialog) {
			BrowserDialog = browserDialog;
			ExecuteCommand = new RelayCommand(Execute);
			SetDeployOperationCommand = new RelayCommand<DeployOperation>(SetDeployOperation);
			Messenger.Default.Register<StartDeployOperationEventArgs>(this, StartDeployOperation);
		}

		private void StartDeployOperation(StartDeployOperationEventArgs args) {
			try {
				var serverName = args.Args[DeployArgumentNameConstant.ServerName];
				var userLogin = args.Args[DeployArgumentNameConstant.UserLogin];
				var userPassword = args.Args[DeployArgumentNameConstant.UserPassword];
				var databaseName = args.Args[DeployArgumentNameConstant.DatabaseName];
				var backupPath = args.Args[DeployArgumentNameConstant.BackupPath];
				IDbExecutor executor = new OracleExecutor(serverName, userLogin, userPassword);
				executor.RestoreDb(databaseName, backupPath);
			} catch (Exception ex) {
				BrowserDialog.ShowErrorMessage(ex.Message);
			}
		}

		private void SetDeployOperation(DeployOperation operation) {
			Operation = operation;
		}

		private void Execute() {
			Messenger.Default.Send(Operation);
		}
	}
}
