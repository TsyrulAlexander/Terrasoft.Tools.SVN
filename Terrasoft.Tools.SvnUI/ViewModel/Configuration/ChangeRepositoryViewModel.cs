using System.Collections.Generic;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Terrasoft.Tools.SvnUI.Model.Constant;
using Terrasoft.Tools.SvnUI.Model.Enums;
using Terrasoft.Tools.SvnUI.Model.File;
using Terrasoft.Tools.SvnUI.Model.Property;
using Terrasoft.Tools.SvnUI.Properties;

namespace Terrasoft.Tools.SvnUI.ViewModel.Configuration {
	public class ChangeRepositoryViewModel : BaseConfigurationViewModel {
		private StringProperty _pkgPath;
		private StringProperty _url;
		private StringProperty _svnLogin;
		private StringProperty _svnPassword;
		public StringProperty PkgPath {
			get => _pkgPath;
			set {
				_pkgPath = value;
				RaisePropertyChanged();
			}
		}
		public StringProperty Url {
			get => _url;
			set {
				_url = value;
				RaisePropertyChanged();
			}
		}
		public StringProperty SvnLogin {
			get => _svnLogin;
			set {
				_svnLogin = value;
				RaisePropertyChanged();
			}
		}
		public StringProperty SvnPassword {
			get => _svnPassword;
			set {
				_svnPassword = value;
				RaisePropertyChanged();
			}
		}
		public RelayCommand OpenPkgPathDialogCommand { get; }
		public ChangeRepositoryViewModel(IBrowserDialog browserDialog): base(browserDialog) {
			PkgPath = new StringProperty(Resources.PkgPath, true, ConfigurationArgumentNameConstant.PkgPath) {
				Value = AppSetting.DefPkgPath
			};
			Url = new StringProperty(Resources.UrlPath, true, ConfigurationArgumentNameConstant.UrlPath) {
				Value = AppSetting.DefBranchReleaseUrl
			};
			SvnLogin = new StringProperty(Resources.SvnUser, true, ConfigurationArgumentNameConstant.UserLogin) {
				Value = AppSetting.DefSvnUser
			};
			SvnPassword =
				new StringProperty(Resources.SvnPassword, true, ConfigurationArgumentNameConstant.UserPassword) {
					Value = AppSetting.DefSvnPassword
				};
			OpenPkgPathDialogCommand = new RelayCommand(OpenPkgPathDialog);

		}
		private void OpenPkgPathDialog() {
			var path = BrowserDialog.SelectFilder(PkgPath.Value);
			if (path != null) {
				PkgPath.Value = path;
			}
		}
		protected override IEnumerable<BaseProperty> GetProperties() {
			return new[] {
				PkgPath,
				Url,
				SvnLogin,
				SvnPassword
			};
		}
		protected override ConfigurationOperation GetOperation() {
			return ConfigurationOperation.ChangeRepository;
		}
	}
}