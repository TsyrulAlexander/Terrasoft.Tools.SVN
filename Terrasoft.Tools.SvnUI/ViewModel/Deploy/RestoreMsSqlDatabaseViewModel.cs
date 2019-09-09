using Terrasoft.Tools.SvnUI.Model.Constant;
using Terrasoft.Tools.SvnUI.Model.Property;
using Terrasoft.Tools.SvnUI.Properties;

namespace Terrasoft.Tools.SvnUI.ViewModel.Deploy {
	public class RestoreMsSqlDatabaseViewModel : BaseRestoreDatabaseViewModel {
		private StringProperty _serverName;
		private StringProperty _userLogin;
		private StringProperty _userPassword;
		private StringProperty _databaseName;
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

		public RestoreMsSqlDatabaseViewModel() {
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
		}
	}
}
