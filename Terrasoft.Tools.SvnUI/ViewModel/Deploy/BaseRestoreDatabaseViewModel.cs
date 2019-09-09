using Terrasoft.Tools.SvnUI.Model.Constant;
using Terrasoft.Tools.SvnUI.Model.Enums;
using Terrasoft.Tools.SvnUI.Model.Property;
using Terrasoft.Tools.SvnUI.Properties;

namespace Terrasoft.Tools.SvnUI.ViewModel.Deploy {
	public class BaseRestoreDatabaseViewModel : BaseViewModel {
		private EnumProperty<FilePathType> _backupFilePathType;
		private StringProperty _backupeLocalPath;
		private StringProperty _backupeFtpPath;
		private StringProperty _ftpLogin;
		private StringProperty _ftpPassword;
		private StringProperty _ftpTempFile;
		public EnumProperty<FilePathType> BackupFilePathType {
			get => _backupFilePathType;
			set {
				_backupFilePathType = value;
				RaisePropertyChanged();
			}
		}
		public StringProperty BackupLocalPath {
			get => _backupeLocalPath;
			set {
				_backupeLocalPath = value;
				RaisePropertyChanged();
			}
		}
		public StringProperty BackupFtpPath {
			get => _backupeFtpPath;
			set {
				_backupeFtpPath = value;
				RaisePropertyChanged();
			}
		}
		public StringProperty FtpLogin {
			get => _ftpLogin;
			set {
				_ftpLogin = value;
				RaisePropertyChanged();
			}
		}
		public StringProperty FtpPassword {
			get => _ftpPassword;
			set {
				_ftpPassword = value;
				RaisePropertyChanged();
			}
		}
		public StringProperty FtpTempFile {
			get => _ftpTempFile;
			set {
				_ftpTempFile = value;
				RaisePropertyChanged();
			}
		}

		public BaseRestoreDatabaseViewModel() {
			BackupFilePathType = new EnumProperty<FilePathType>(Resources.BackupFilePathType, true, DeployArgumentNameConstant.BackupPathType) {
				Value = AppSetting.DefBackupPathType
			};
			BackupLocalPath = new StringProperty(Resources.BackupPath, true, DeployArgumentNameConstant.BackupPath) {
				Value = AppSetting.DefBackupPath
			};
			BackupFtpPath = new StringProperty(Resources.BackupFtpPath, true, DeployArgumentNameConstant.BackupFtpPath) {
				Value = AppSetting.DefBackupFtpPath
			};
			FtpLogin = new StringProperty(Resources.FtpLogin, true, DeployArgumentNameConstant.FtpLogin) {
				Value = AppSetting.DefFtpLogin
			};
			FtpPassword = new StringProperty(Resources.FtpPassword, true, DeployArgumentNameConstant.FtpPassword) {
				Value = AppSetting.DefFtpPassword
			};
			FtpTempFile = new StringProperty(Resources.FtpTempFile, true, DeployArgumentNameConstant.FtpTempFile) {
				Value = AppSetting.DefFtpTempFile
			};
		}
	}
}
