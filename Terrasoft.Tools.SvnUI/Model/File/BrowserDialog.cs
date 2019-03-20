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
	}
}
