using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Threading;
using Terrasoft.Core;
using Terrasoft.Tools.SvnUI.Model.File;
using Terrasoft.Tools.SvnUI.Model.Log;

namespace Terrasoft.Tools.SvnUI.ViewModel
{
    public class LogViewModel : ViewModelBase
    {
        private LogInfo _selected;

        public LogViewModel(ILogger logger, IBrowserDialog browserDialog) {
            Logger = logger;
            BrowserDialog = browserDialog;
            ((UiLogger) logger).Execute += OnLogExecute;
            LogInfoCollection = new ObservableCollection<LogInfo>();
            ClearCommand = new RelayCommand(Clear);
            CopyCommand = new RelayCommand(CopyLog);
            SaveLogCommand = new RelayCommand(SaveLog);
        }

        public ILogger Logger { get; }
        public IBrowserDialog BrowserDialog { get; }
        public ObservableCollection<LogInfo> LogInfoCollection { get; }
        public RelayCommand ClearCommand { get; set; }
        public RelayCommand CopyCommand { get; set; }
        public RelayCommand SaveLogCommand { get; set; }

        public LogInfo Selected {
            get => _selected;
            set {
                _selected = value;
                RaisePropertyChanged();
            }
        }

        public void SaveLog() {
            string path = BrowserDialog.SaveFile(null, "Text documents (.txt)|*.txt");
            if (string.IsNullOrWhiteSpace(path)) {
                return;
            }

            SaveLog(path);
        }

        private void SaveLog(string path) {
            using (var writer = new StreamWriter(path, false, Encoding.ASCII)) {
                writer.Write(GetLogCollectionToString());
            }
        }

        private string GetLogCollectionToString() {
            var builder = new StringBuilder();
            foreach (LogInfo logInfo in LogInfoCollection) {
                builder.AppendLine(logInfo.ToString());
            }

            return builder.ToString();
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