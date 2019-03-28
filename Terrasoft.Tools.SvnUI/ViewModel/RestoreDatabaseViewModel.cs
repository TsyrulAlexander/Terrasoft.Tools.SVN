using System.Collections.Generic;
using System.Linq;
using GalaSoft.MvvmLight.CommandWpf;
using Terrasoft.Tools.SvnUI.Model;
using Terrasoft.Tools.SvnUI.Model.Constant;
using Terrasoft.Tools.SvnUI.Model.Enums;
using Terrasoft.Tools.SvnUI.Model.File;
using Terrasoft.Tools.SvnUI.Model.Property;
using Terrasoft.Tools.SvnUI.Properties;

namespace Terrasoft.Tools.SvnUI.ViewModel
{
	public class RestoreDatabaseViewModel : BaseDeployViewModel {
		private StringProperty _serverName;
		private StringProperty _userLogin;
		private StringProperty _userPassword;
		private StringProperty _databaseName;
		private StringProperty _backupePath;
		public StringProperty ServerName {
			get => _serverName;
			set {
				_serverName = value;
				RaisePropertyChanged();
			}
		}
		public StringProperty UserLogin {
			get => _userLogin;
			set {
				_userLogin = value;
				RaisePropertyChanged();
			}
		}
		public StringProperty UserPassword {
			get => _userPassword;
			set {
				_userPassword = value;
				RaisePropertyChanged();
			}
		}
		public StringProperty DatabaseName {
			get => _databaseName;
			set {
				_databaseName = value;
				RaisePropertyChanged();
			}
		}
		public StringProperty BackupPath {
			get => _backupePath;
			set {
				_backupePath = value;
				RaisePropertyChanged();
			}
		}

		public RelayCommand SelectBackupCommand { get; set; }

		public RestoreDatabaseViewModel(IBrowserDialog browserDialog) : base(browserDialog) {
			ServerName = new StringProperty(Resources.ServerName, true, DeployArgumentNameConstant.ServerName) {
				Value = AppSetting.DefDbServerName
			};
			UserLogin = new StringProperty(Resources.UserLogin, true, DeployArgumentNameConstant.UserLogin) {
				Value = AppSetting.DefDbServerUserLogin
			};
			UserPassword = new StringProperty(Resources.UserPassword, true, DeployArgumentNameConstant.UserPassword) {
				Value = AppSetting.DefDbServerUserPassword
			};
			DatabaseName = new StringProperty(Resources.DatabaseName, true, DeployArgumentNameConstant.DatabaseName) {
				Value = AppSetting.DefDatabaseName
			};
			BackupPath = new StringProperty(Resources.BackupPath, true, DeployArgumentNameConstant.BackupPath) {
				Value = AppSetting.DefBackupPath
			};
			SelectBackupCommand = new RelayCommand(SelectBackup);
		}

		private void SelectBackup() {
			var backPath = BrowserDialog.SelectFile("Backup file (*.bak)|*.bak");
			if (string.IsNullOrWhiteSpace(backPath)) {
				return;
			}
			BackupPath.Value = backPath;
		}

		protected override IEnumerable<BaseProperty> GetProperties() {
			return base.GetProperties().Concat(new [] {
				ServerName, UserLogin, UserPassword, DatabaseName, BackupPath
			});
		}

		protected override DeployOperation GetOperation() {
			return DeployOperation.RestoreDb;
		}
	}
}
