using System;
using System.Windows.Forms;

namespace Terrasoft.Tools.SvnUI.Model.File {
	public class BrowserDialog : IBrowserDialog {
		public string SelectFilder(string path) {
			using (var dialog = new FolderBrowserDialog()) {
				dialog.SelectedPath = path;
				DialogResult result = dialog.ShowDialog();
				if (result == DialogResult.OK) {
					return dialog.SelectedPath;
				}
			}
			return null;
		}

		public void ShowModalBox(string message, string caption = "") {
			MessageBox.Show(message, caption);
		}

		public string SaveFile(string directory = "", string filter = "") {
			using (var dialog = new SaveFileDialog()) {
				if (string.IsNullOrWhiteSpace(directory)) {
					directory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
				}
				dialog.InitialDirectory = directory;
				dialog.Filter = filter;
				if (dialog.ShowDialog() == DialogResult.OK) {
					return dialog.FileName;
				}
				return null;
			}
		}
	}
}
