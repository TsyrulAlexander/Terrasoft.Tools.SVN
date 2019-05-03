using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using Terrasoft.Core.DeployApp.Database;
using Terrasoft.Core.DeployApp.Database.Oracle;
using Terrasoft.Tools.SvnUI.Model.Constant;
using Terrasoft.Tools.SvnUI.Model.Enums;
using Terrasoft.Tools.SvnUI.Model.EventArgs;
using Terrasoft.Tools.SvnUI.Model.File;

namespace Terrasoft.Tools.SvnUI.ViewModel
{
    public class DeployAppViewModel : ViewModelBase
    {
        private bool _inProgress;
        private DeployOperation _operation = DeployOperation.RestoreDb;

        public DeployAppViewModel(IBrowserDialog browserDialog) {
            BrowserDialog = browserDialog;
            ExecuteCommand = new RelayCommand(Execute);
            SetDeployOperationCommand = new RelayCommand<DeployOperation>(SetDeployOperation);
            Messenger.Default.Register<StartDeployOperationEventArgs>(this, StartDeployOperation);
        }

        public IBrowserDialog BrowserDialog { get; }

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

        private void StartDeployOperation(StartDeployOperationEventArgs args) {
            try {
                string serverName = args.Args[DeployArgumentNameConstant.ServerName];
                string userLogin = args.Args[DeployArgumentNameConstant.UserLogin];
                string userPassword = args.Args[DeployArgumentNameConstant.UserPassword];
                string databaseName = args.Args[DeployArgumentNameConstant.DatabaseName];
                string backupPath = args.Args[DeployArgumentNameConstant.BackupPath];
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