using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Terrasoft.Tools.SvnUI.Model.File
{
	public interface IBrowserDialog {
		string SelectFilder(string path);
		void ShowModalBox(string message, string caption = "");
	}
}
