using System;
using System.Windows.Forms;

namespace Terrasoft.Tools.SvnUI.Model.File {
	public class BrowserDialog : IBrowserDialog {
		public string SelectFilder(string path) {
			using (var dialog = new FolderBrowserDialog()) {
				dialog.SelectedPath = path;
				var result = dialog.ShowDialog();
				if (result == DialogResult.OK) {
					return dialog.SelectedPath;
				}
			}
			return null;
		}

		public void ShowInformationMessage(string message, string caption = "") {
			MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		public void ShowErrorMessage(string message, string caption = "") {
			MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
		}

		public bool ShowModalYesNo(string message, string caption = "") {
			return MessageBox.Show(message, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes;
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

		public string SelectFile(string filter = "") {
			using (var dialog = new OpenFileDialog()) {
				dialog.Filter = filter;
				if (dialog.ShowDialog() == DialogResult.OK) {
					return dialog.FileName;
				}
			}
			return null;
		}
	}
}
