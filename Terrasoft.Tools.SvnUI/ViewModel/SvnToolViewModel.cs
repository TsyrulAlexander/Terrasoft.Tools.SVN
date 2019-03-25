using System;
using System.IO;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using Terrasoft.Core;
using Terrasoft.Core.SVN;
using Terrasoft.Tools.SvnUI.Model;
using Terrasoft.Tools.SvnUI.Model.File;
using Terrasoft.Tools.SvnUI.Properties;

namespace Terrasoft.Tools.SvnUI.ViewModel {
	public class SvnToolViewModel : ViewModelBase {
		public ILogger Logger { get; }
		public IBrowserDialog BrowserDialog { get; }
		private SvnOperation _svnOperation = SvnOperation.CreateFeature;
		private bool _inProgress;
		public SvnOperation SvnOperation {
			get => _svnOperation;
			set {
				_svnOperation = value;
				RaisePropertyChanged();
			}
		}
		public bool InProgress {
			get => _inProgress;
			set {
				_inProgress = value;
				RaisePropertyChanged();
			}
		}
		public RelayCommand RunCommand { get; set; }
		public RelayCommand<SvnOperation> SetSvnOperationCommand { get; set; }

		public SvnToolViewModel(ILogger logger, IBrowserDialog browserDialog) {
			Logger = logger;
			BrowserDialog = browserDialog;
			RunCommand = new RelayCommand(Run, CanRun);
			SetSvnOperationCommand = new RelayCommand<SvnOperation>(SetSvnOperation);
			Messenger.Default.Register<SvnOperationConfig>(this, StartSvnOperation);
		}

		private void SetSvnOperation(SvnOperation operation) {
			SvnOperation = operation;
		}

		public async void StartSvnOperation(SvnOperationConfig config) {
			await Task.Run(() => StartSvnOperationAsync(config));
		}

		private void StartSvnOperationAsync(SvnOperationConfig config) {
			try {
				SetProgressState(true);
				using (var svnUtils = new SvnUtils(config.Arguments, Logger)) {
					switch (config.SvnOperation) {
						case SvnOperation.CreateFeature:
							var workingCopyPath = config.Arguments[SvnUtilsBase.WorkingCopyPathOptionName];
							Directory.CreateDirectory(workingCopyPath);
							svnUtils.CreateFeature();
							break;
						case SvnOperation.UpdateFeature:
							CheckoutWorkingCopy(svnUtils, config);
							if (svnUtils.UpdateFromReleaseBranch() && svnUtils.CommitIfNoError) {
								svnUtils.CommitChanges(true);
							}
							break;
						case SvnOperation.FinishFeature:
							CheckoutWorkingCopy(svnUtils, config);
							svnUtils.ReintegrationMergeToBaseBranch();
							break;
						case SvnOperation.CloseFeature:
							CheckoutWorkingCopy(svnUtils, config);
							svnUtils.DeleteClosedFeature();
							break;
						case SvnOperation.FixFeature:
							CheckoutWorkingCopy(svnUtils, config);
							svnUtils.FixBranch();
							break;
						default:
							throw new NotImplementedException(nameof(config.SvnOperation));
					}
				}
				BrowserDialog.ShowModalBox(Resources.SvnOperationComplite);
			} catch (Exception exception) {
				Logger.LogError(exception.Message);
				BrowserDialog.ShowModalBox(exception.Message, Resources.Error);
			} finally {
				SetProgressState(false);
			}
		}

		protected virtual void CheckoutWorkingCopy(SvnUtils svnUtils, SvnOperationConfig config) {
			var workingCopyPath = config.Arguments[SvnUtilsBase.WorkingCopyPathOptionName];
			var repositoryPath = config.Arguments[SvnUtilsBase.BranchFeatureUrlOptionName];
			if (File.Exists(workingCopyPath) || string.IsNullOrWhiteSpace(repositoryPath)) {
				return;
			}
			svnUtils.CheckoutWorkingCopy(workingCopyPath, repositoryPath);
		}

		private void SetProgressState(bool state) {
			DispatcherHelper.CheckBeginInvokeOnUI(() => InProgress = state);
		}

		private bool CanRun() {
			return SvnOperation != SvnOperation.NaN && InProgress == false;
		}

		public void Run() {
			Messenger.Default.Send(SvnOperation);
		}
	}
}