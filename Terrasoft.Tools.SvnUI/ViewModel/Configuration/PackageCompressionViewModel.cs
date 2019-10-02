using System.Collections.Generic;
using System.Linq;
using GalaSoft.MvvmLight.CommandWpf;
using Terrasoft.Tools.SvnUI.Model.Constant;
using Terrasoft.Tools.SvnUI.Model.Enums;
using Terrasoft.Tools.SvnUI.Model.File;
using Terrasoft.Tools.SvnUI.Model.Property;
using Terrasoft.Tools.SvnUI.Properties;

namespace Terrasoft.Tools.SvnUI.ViewModel.Configuration {
	public class PackageCompressionViewModel : BaseConfigurationViewModel {
		private StringProperty _packagePath;
		private StringProperty _savePath;
		public StringProperty PackagePath {
			get => _packagePath;
			set {
				_packagePath = value;
				RaisePropertyChanged();
			}
		}
		public StringProperty SavePath {
			get => _savePath;
			set {
				_savePath = value;
				RaisePropertyChanged();
			}
		}
		public RelayCommand SelectPackagePathCommand { get; set; }
		public RelayCommand SelectSavePathCommand { get; set; }
		public PackageCompressionViewModel(IBrowserDialog browserDialog) : base(browserDialog) {
			PackagePath = new StringProperty(Resources.PkgPath, true, ConfigurationArgumentNameConstant.PkgPath) {
				Value = AppSetting.DefPkgPath
			};
			SavePath = new StringProperty(Resources.SavePath, true, ConfigurationArgumentNameConstant.SavePath) {
				Value = AppSetting.DefSaveTempDirectory
			};
			SelectPackagePathCommand = new RelayCommand(SelectPackagePath);
			SelectSavePathCommand = new RelayCommand(SelectSavePath);
		}
		private void SelectSavePath() {
			var path = BrowserDialog.SelectFilder(SavePath.Value);
			if (!string.IsNullOrWhiteSpace(path)) {
				SavePath.Value = path;
			}
		}
		private void SelectPackagePath() {
			var path = BrowserDialog.SelectFilder(PackagePath.Value);
			if (!string.IsNullOrWhiteSpace(path)) {
				PackagePath.Value = path;
			}
		}
		protected override IEnumerable<BaseProperty> GetProperties() {
			return base.GetProperties().Concat(new[] {
				SavePath,
				PackagePath
			});
		}
		protected override ConfigurationOperation GetOperation() {
			return ConfigurationOperation.PackageCompression;
		}
	}
}