using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Views;
using Terrasoft.Core;
using Terrasoft.Core.Version;
using Terrasoft.Tools.SvnUI.Model;
using Terrasoft.Tools.SvnUI.Model.File;
using Terrasoft.Tools.SvnUI.Properties;

namespace Terrasoft.Tools.SvnUI.ViewModel
{
	public class MainViewModel : ViewModelBase {
		public IBrowserDialog BrowserDialog { get; }
		public ILogger Logger { get; }
		private OperationType _operationType;
		private bool _isActualVersion = true;
		public OperationType OperationType {
			get => _operationType;
			set {
				_operationType = value;
				RaisePropertyChanged();
			}
		}
		public RelayCommand<OperationType> SetOperationTypeCommand { get; set; }
		public RelayCommand UpdateAppCommand { get; set; }
		public bool IsActualVersion {
			get => _isActualVersion;
			set {
				_isActualVersion = value;
				RaisePropertyChanged();
			}
		}

		public MainViewModel(IBrowserDialog browserDialog, ILogger logger) {
			BrowserDialog = browserDialog;
			Logger = logger;
			SetOperationTypeCommand = new RelayCommand<OperationType>(SetOperationType);
			UpdateAppCommand = new RelayCommand(UpdateApp);
			if (AppSetting.CheckNewVersionIsAppStart) {
				SetIsActualVersion();
			}
		}

		private async void SetIsActualVersion() {
			try {
				IsActualVersion = await GetIsCurrentVersionIsActual();
			} catch (Exception ex) {
				Logger.LogError(ex.Message);
			}
		}

		private void UpdateApp() {
			if (BrowserDialog.ShowModalYesNo(Resources.UpdateAppText, string.Empty)) {
				UpdateAppVersion();
			}
		}

		private async void UpdateAppVersion() {
			try {
				if (await GetIsCurrentVersionIsActual()) {
					BrowserDialog.ShowInformationMessage(Resources.CurrentVersionIsActual);
					return;
				}
				await AppVersionUtilities.UpdateApp();
				Application.Current.Shutdown();
			} catch (Exception ex) {
				BrowserDialog.ShowErrorMessage(ex.Message);
			}
		}

		private async Task<bool> GetIsCurrentVersionIsActual() {
			var currentVarsion = AppSetting.LatestVersionId;
			var repositoryVersion = await AppVersionUtilities.GetVersionIdAsync();
			return currentVarsion == repositoryVersion;
		}

		private void SetOperationType(OperationType operationType) {
			OperationType = operationType;
		}
	}
}