using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using Terrasoft.Tools.SvnUI.Model;
using Terrasoft.Tools.SvnUI.Model.File;
using Terrasoft.Tools.SvnUI.Model.Property;
using Terrasoft.Tools.SvnUI.Properties;

namespace Terrasoft.Tools.SvnUI.ViewModel
{
	public abstract class BaseSvnOperationViewModel: ViewModelBase
	{
		public IBrowserDialog BrowserDialog { get; }
		private StringProperty _svnUser;
		private StringProperty _svnPassword;
		private StringProperty _workingCopyPath;

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

		public RelayCommand SelectWorkingCopyPathCommand { get; set; }

		public BaseSvnOperationViewModel(IBrowserDialog browserDialog) {
			BrowserDialog = browserDialog;
			SvnUser = new StringProperty(Resources.SvnUser, true, "SvnUser") {
				Description = Resources.SvnUserDescription,
				Value = AppSetting.DefSvnUser
			};
			SvnPassword = new StringProperty(Resources.SvnPassword, true, "SvnPassword") {
				Description = Resources.SvnPasswordDescription,
				Value = AppSetting.DefSvnPassword
			};
			;
			WorkingCopyPath = new StringProperty(Resources.WorkingCopyPath, true, "WorkingCopyPath") {
				Description = Resources.WorkingCopyPathDescription,
				Value = AppSetting.DefWorkingCopyPath
			};
			SelectWorkingCopyPathCommand = new RelayCommand(SelectWorkingCopyPath);
			Messenger.Default.Register<SvnOperation>(this, OnRunSvnOperation);
		}

		private void SelectWorkingCopyPath() {
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
			var svnArguments = GetSvnArguments();
			StartSvnOperation(svnArguments);
		}

		protected virtual void StartSvnOperation(Dictionary<string, string> args) {
			Messenger.Default.Send(new SvnOperationConfig {
				SvnOperation = GetSvnOperation(),
				Arguments = args
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
			return new BaseProperty[] {SvnUser, SvnPassword, WorkingCopyPath};
		}
	}
}
