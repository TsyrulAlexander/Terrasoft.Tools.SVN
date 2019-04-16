using System;

namespace Terrasoft.Tools.SvnUI.Model.File {
	public interface IBrowserDialog {
		string SelectFilder(string path);
		void ShowInformationMessage(string message, string caption = "");
		void ShowErrorMessage(string message, string caption = "");
		bool ShowModalYesNo(string message, string caption = "");
		string SaveFile(string directory = "", string filter = "");
		string SelectFile(string filter = "");
	}
}
