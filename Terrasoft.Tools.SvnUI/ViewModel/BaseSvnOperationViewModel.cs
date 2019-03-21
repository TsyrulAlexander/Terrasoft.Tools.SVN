using System;
using System.Collections.Generic;
using System.IO;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using Terrasoft.Core.SVN;
using Terrasoft.Tools.SvnUI.Model;
using Terrasoft.Tools.SvnUI.Model.File;
using Terrasoft.Tools.SvnUI.Model.Property;
using Terrasoft.Tools.SvnUI.Properties;

namespace Terrasoft.Tools.SvnUI.ViewModel {
	public abstract class BaseSvnOperationViewModel : ViewModelBase {
		protected IBrowserDialog BrowserDialog { get; }
		private StringProperty _svnUser;
		private StringProperty _svnPassword;
		private StringProperty _workingCopyPath;
		private StringProperty _branchFeatureUrl;

		public StringProperty SvnUser {
			get => _svnUser;
			set {
				_svnUser = value;
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
		public StringProperty WorkingCopyPath {
			get => _workingCopyPath;
			set {
				_workingCopyPath = value;
				RaisePropertyChanged();
			}
		}

		public StringProperty BranchFeatureUrl {
			get => _branchFeatureUrl;
			set {
				_branchFeatureUrl = value;
				RaisePropertyChanged();
			}
		}

		public RelayCommand SelectWorkingCopyPathCommand { get; set; }

		public BaseSvnOperationViewModel(IBrowserDialog browserDialog) {
			BrowserDialog = browserDialog;
			SelectWorkingCopyPathCommand = new RelayCommand(SelectWorkingCopyPath);
			Messenger.Default.Register<SvnOperation>(this, OnRunSvnOperation);
			InitPropertyValues();
		}

		private void InitPropertyValues() {
			SvnUser = new StringProperty(Resources.SvnUser, true, SvnUtilsBase.SvnUserOptionName) {
				Description = Resources.SvnUserDescription, Value = AppSetting.DefSvnUser
			};
			SvnPassword = new StringProperty(Resources.SvnPassword, true, SvnUtilsBase.SvnPasswordOptionName) {
				Description = Resources.SvnPasswordDescription, Value = AppSetting.DefSvnPassword
			};
			WorkingCopyPath =
				new StringProperty(Resources.WorkingCopyPath, true, SvnUtilsBase.WorkingCopyPathOptionName) {
					Description = Resources.WorkingCopyPathDescription, Value = AppSetting.DefWorkingCopyPath
				};
			BranchFeatureUrl =
				new StringProperty(Resources.BranchFeatureUrl, true, SvnUtilsBase.BranchFeatureUrlOptionName) {
					Description = Resources.BranchFeatureUrlDescription
				};
			WorkingCopyPath.PropertyChanged += (sender, args) => {
				if (args.PropertyName == nameof(WorkingCopyPath.Value)) {
					SetBranchFeatureUrlFromWorkingCopyPath();
				}
			};
			SetBranchFeatureUrlFromWorkingCopyPath();
		}

		protected virtual void SetBranchFeatureUrlFromWorkingCopyPath() {
			if (string.IsNullOrWhiteSpace(WorkingCopyPath?.Value) || !Directory.Exists(WorkingCopyPath.Value)) {
				return;
			}
			try {
				BranchFeatureUrl.Value = SvnUtils.GetRepositoryPathWithFolder(WorkingCopyPath.Value);
			} catch {
				BranchFeatureUrl.Value = string.Empty;
			}
		}

		protected virtual void SelectWorkingCopyPath() {
			var path = BrowserDialog.SelectFilder(WorkingCopyPath.Value);
			if (path != null) {
				WorkingCopyPath.Value = path;
			}
		}

		protected virtual void OnRunSvnOperation(SvnOperation operation) {
			if (operation != GetSvnOperation()) {
				return;
			}
			if (!ValidateParameters(out string message)) {
				BrowserDialog.ShowModalBox(message);
				return;
			}
			CreateIfNotExistWorkingCopyDirectory();
			var svnArguments = GetSvnArguments();
			StartSvnOperation(svnArguments);
		}

		protected virtual void CreateIfNotExistWorkingCopyDirectory() {
			Directory.CreateDirectory(WorkingCopyPath.Value);
		}

		protected virtual void StartSvnOperation(Dictionary<string, string> args) {
			Messenger.Default.Send(new SvnOperationConfig {
				SvnOperation = GetSvnOperation(), Arguments = args
			});
		}

		protected virtual bool ValidateParameters(out string message) {
			var properties = GetOperationProperties();
			foreach (var property in properties) {
				if (!property.IsValid(out message)) {
					return false;
				}
			}
			message = string.Empty;
			return true;
		}

		protected virtual Dictionary<string, string> GetSvnArguments() {
			var args = new Dictionary<string, string>();
			var properties = GetOperationProperties();
			foreach (var property in properties) {
				var operationKey = (string) property.Tag;
				args.Add(operationKey.ToLower(), property.ToString());
			}
			return args;
		}

		public abstract SvnOperation GetSvnOperation();

		protected virtual IEnumerable<BaseProperty> GetOperationProperties() {
			return new BaseProperty[] {
				SvnUser, SvnPassword, WorkingCopyPath, BranchFeatureUrl
			};
		}
	}
}
