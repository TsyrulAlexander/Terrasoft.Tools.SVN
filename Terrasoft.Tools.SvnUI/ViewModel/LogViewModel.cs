using System.Collections.ObjectModel;
using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Threading;
using Terrasoft.Core.SVN;
using Terrasoft.Tools.SvnUI.Model;

namespace Terrasoft.Tools.SvnUI.ViewModel {
	public class LogViewModel : ViewModelBase {
		private LogInfo _selected;
		public ILogger Logger { get; }
		public ObservableCollection<LogInfo> LogInfoCollection { get; }
		public RelayCommand ClearCommand { get; set; }
		public RelayCommand CopyCommand { get; set; }

		public LogInfo Selected {
			get => _selected;
			set {
				_selected = value;
				RaisePropertyChanged();
			}
		}

		public LogViewModel(ILogger logger) {
			Logger = logger;
			((UILogger) logger).Execute += OnLogExecute;
			LogInfoCollection = new ObservableCollection<LogInfo>();
			ClearCommand = new RelayCommand(Clear);
			CopyCommand = new RelayCommand(CopyLog);
		}

		private void OnLogExecute(LogInfo obj) {
			DispatcherHelper.CheckBeginInvokeOnUI(() => AddLog(obj));
		}

		public void AddLog(LogInfo logInfo) {
			LogInfoCollection.Add(logInfo);
		}

		public void CopyLog() {
			if (Selected == null || string.IsNullOrWhiteSpace(Selected.Message)) {
				return;
			}
			Clipboard.SetText(Selected.Message);
		}

		public void Clear() {
			LogInfoCollection.Clear();
		}
	}
}