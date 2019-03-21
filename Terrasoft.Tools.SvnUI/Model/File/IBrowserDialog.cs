using System;

namespace Terrasoft.Tools.SvnUI.Model.File {
	public interface IBrowserDialog {
		string SelectFilder(string path);
		void ShowModalBox(string message, string caption = "");
		string SaveFile(string directory = "", string filter = "");
	}
}
