using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using Terrasoft.Core.SVN;
using Terrasoft.Tools.SvnUI.Model;
using Terrasoft.Tools.SvnUI.Model.Converter;

namespace Terrasoft.Tools.SvnUI.ViewModel
{
	public class MainViewModel : ViewModelBase {
		public ILogger Logger { get; }
		private KeyValuePair<SvnOperation, string> _svnOperation;

		public KeyValuePair<SvnOperation, string> SvnOperation {
			get => _svnOperation;
			set {
				_svnOperation = value;
				RaisePropertyChanged();
			}
		}

		public Dictionary<SvnOperation, string> SvnOperations { get; }

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
			Messenger.Default.Register<SvnOperationConfig>(this, StartSvnOperation);
		}

		public async void StartSvnOperation(SvnOperationConfig config) {
			await Task.Run(() => StartSvnOperationAsync(config));
		}

		private void StartSvnOperationAsync(SvnOperationConfig config) {
			try {
				using (var svnUtils = new SvnUtils(config.Arguments, Logger)) {
					switch (config.SvnOperation) {
						case Model.SvnOperation.CreateFeature:
							svnUtils.CreateFeature();
							break;
						case Model.SvnOperation.UpdateFeature:
							if (svnUtils.UpdateFromReleaseBranch() && svnUtils.CommitIfNoError) {
								svnUtils.CommitChanges(true);
							}
							break;
						case Model.SvnOperation.FinishFeature:
							svnUtils.ReintegrationMergeToBaseBranch();
							break;
						case Model.SvnOperation.CloseFeature:
							svnUtils.DeleteClosedFeature();
							break;
						case Model.SvnOperation.FixFeature:
							svnUtils.FixBranch();
							break;
						default:
							throw new NotImplementedException(nameof(config.SvnOperation));
					}
				}
			} catch (Exception exception) {
				Logger.LogError("Error: ", exception.Message);
			}
		}

		private bool CanRun() {
			return SvnOperation.Key != Model.SvnOperation.NaN;
		}

		public void Run() {
			Messenger.Default.Send(SvnOperation.Key);
		}
	}
}