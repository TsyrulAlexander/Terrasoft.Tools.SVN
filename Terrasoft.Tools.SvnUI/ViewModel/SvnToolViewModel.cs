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
using Terrasoft.Tools.SvnUI.Model.Enums;
using Terrasoft.Tools.SvnUI.Model.EventArgs;
using Terrasoft.Tools.SvnUI.Model.File;
using Terrasoft.Tools.SvnUI.Properties;

namespace Terrasoft.Tools.SvnUI.ViewModel {
	public class SvnToolViewModel : BaseViewModel {
		public ILogger Logger { get; }
		public IBrowserDialog BrowserDialog { get; }
		private SvnOperation _svnOperation = SvnOperation.CreateFeature;
		public SvnOperation SvnOperation {
			get => _svnOperation;
			set {
				_svnOperation = value;
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
			Messenger.Default.Register<StartSvnOperationEventArgs>(this, StartSvnOperation);
		}

		private void SetSvnOperation(SvnOperation operation) {
			SvnOperation = operation;
		}

		public async void StartSvnOperation(StartSvnOperationEventArgs eventArgs) {
			await Task.Run(() => StartSvnOperationAsync(eventArgs));
		}

		private void StartSvnOperationAsync(StartSvnOperationEventArgs eventArgs) {
			try {
				SetProgressState(true);
				using (var svnUtils = new SvnUtils(eventArgs.Arguments, Logger)) {
					switch (eventArgs.SvnOperation) {
						case SvnOperation.CreateFeature:
							var workingCopyPath = eventArgs.Arguments[SvnUtilsBase.WorkingCopyPathOptionName];
							Directory.CreateDirectory(workingCopyPath);
							svnUtils.CreateFeature();
							break;
						case SvnOperation.UpdateFeature:
							CheckoutWorkingCopy(svnUtils, eventArgs);
							if (svnUtils.UpdateFromReleaseBranch() && svnUtils.CommitIfNoError) {
								svnUtils.CommitChanges(true);
							}
							break;
						case SvnOperation.FinishFeature:
							CheckoutWorkingCopy(svnUtils, eventArgs);
							svnUtils.ReintegrationMergeToBaseBranch();
							break;
						case SvnOperation.CloseFeature:
							CheckoutWorkingCopy(svnUtils, eventArgs);
							svnUtils.DeleteClosedFeature();
							break;
						case SvnOperation.FixFeature:
							CheckoutWorkingCopy(svnUtils, eventArgs);
							svnUtils.FixBranch();
							break;
						default:
							throw new NotImplementedException(nameof(eventArgs.SvnOperation));
					}
				}
				BrowserDialog.ShowInformationMessage(Resources.SvnOperationComplite);
			} catch (Exception exception) {
				Logger.LogError(exception.Message);
				BrowserDialog.ShowInformationMessage(exception.Message, Resources.Error);
			} finally {
				SetProgressState(false);
			}
		}

		protected virtual void CheckoutWorkingCopy(SvnUtils svnUtils, StartSvnOperationEventArgs eventArgs) {
			var workingCopyPath = eventArgs.Arguments[SvnUtilsBase.WorkingCopyPathOptionName];
			var repositoryPath = eventArgs.Arguments[SvnUtilsBase.BranchFeatureUrlOptionName];
			if (Directory.Exists(workingCopyPath) || string.IsNullOrWhiteSpace(repositoryPath)) {
				return;
			}
			svnUtils.CheckoutWorkingCopy(workingCopyPath, repositoryPath);
		}

		private bool CanRun() {
			return SvnOperation != SvnOperation.NaN && InProgress == false;
		}

		public void Run() {
			Messenger.Default.Send(SvnOperation);
		}
	}
}