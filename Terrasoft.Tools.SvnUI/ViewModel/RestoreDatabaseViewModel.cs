using System.Collections.Generic;
using System.Linq;
using Terrasoft.Tools.SvnUI.Model;
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

		public RestoreDatabaseViewModel(IBrowserDialog browserDialog) : base(browserDialog) {
			ServerName = new StringProperty(Resources.ServerName, true, AppSetting.DefDbServerName);
			UserLogin = new StringProperty(Resources.UserLogin, true, AppSetting.DefDbServerUserLogin);
			UserPassword = new StringProperty(Resources.UserPassword, true, AppSetting.DefDbServerUserPassword);
			DatabaseName = new StringProperty(Resources.DatabaseName, true, AppSetting.DefDatabaseName);
			BackupPath = new StringProperty(Resources.BackupPath, true, AppSetting.DefBackupPath);
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
