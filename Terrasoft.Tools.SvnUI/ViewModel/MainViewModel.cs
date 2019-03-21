using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using Terrasoft.Core.SVN;
using Terrasoft.Tools.SvnUI.Model;
using Terrasoft.Tools.SvnUI.Model.Converter;

namespace Terrasoft.Tools.SvnUI.ViewModel
{
	public class MainViewModel : ViewModelBase {
		public ILogger Logger { get; }
		private KeyValuePair<SvnOperation, string> _svnOperation;
		private bool _inProgress;
		public KeyValuePair<SvnOperation, string> SvnOperation {
			get => _svnOperation;
			set {
				_svnOperation = value;
				RaisePropertyChanged();
			}
		}

		public Dictionary<SvnOperation, string> SvnOperations { get; }
		public bool InProgress {
			get => _inProgress;
			set {
				_inProgress = value;
				RaisePropertyChanged();
			}
		}
		public RelayCommand RunCommand { get; set; }
		public MainViewModel(ILogger logger) {
			Logger = logger;
			RunCommand = new RelayCommand(Run, CanRun);
			SvnOperations = new Dictionary<SvnOperation, string> {
				{ Model.SvnOperation.CreateFeature, EnumDescriptionConverter.GetDescription(Model.SvnOperation.CreateFeature) },
				{ Model.SvnOperation.UpdateFeature, EnumDescriptionConverter.GetDescription(Model.SvnOperation.UpdateFeature) },
				{ Model.SvnOperation.FinishFeature, EnumDescriptionConverter.GetDescription(Model.SvnOperation.FinishFeature) },
				{ Model.SvnOperation.CloseFeature, EnumDescriptionConverter.GetDescription(Model.SvnOperation.CloseFeature) },
				{ Model.SvnOperation.FixFeature, EnumDescriptionConverter.GetDescription(Model.SvnOperation.FixFeature) }
			};
			SvnOperation = SvnOperations.ElementAt(0);
			Messenger.Default.Register<SvnOperationConfig>(this, StartSvnOperation);
		}

		public async void StartSvnOperation(SvnOperationConfig config) {
			await Task.Run(() => StartSvnOperationAsync(config));
		}

		private void StartSvnOperationAsync(SvnOperationConfig config) {
			try {
				SetProgressState(true);
				using (var svnUtils = new SvnUtils(config.Arguments, Logger)) {
					switch (config.SvnOperation) {
						case Model.SvnOperation.CreateFeature:
							var workingCopyPath = config.Arguments[SvnUtilsBase.WorkingCopyPathOptionName];
							Directory.CreateDirectory(workingCopyPath);
							svnUtils.CreateFeature();
							break;
						case Model.SvnOperation.UpdateFeature:
							CheckoutWorkingCopy(svnUtils, config);
							if (svnUtils.UpdateFromReleaseBranch() && svnUtils.CommitIfNoError) {
								svnUtils.CommitChanges(true);
							}
							break;
						case Model.SvnOperation.FinishFeature:
							CheckoutWorkingCopy(svnUtils, config);
							svnUtils.ReintegrationMergeToBaseBranch();
							break;
						case Model.SvnOperation.CloseFeature:
							CheckoutWorkingCopy(svnUtils, config);
							svnUtils.DeleteClosedFeature();
							break;
						case Model.SvnOperation.FixFeature:
							CheckoutWorkingCopy(svnUtils, config);
							svnUtils.FixBranch();
							break;
						default:
							throw new NotImplementedException(nameof(config.SvnOperation));
					}
				}
			} catch (Exception exception) {
				Logger.LogError(exception.Message);
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
			return SvnOperation.Key != Model.SvnOperation.NaN && InProgress == false;
		}

		public void Run() {
			Messenger.Default.Send(SvnOperation.Key);
		}
	}
}