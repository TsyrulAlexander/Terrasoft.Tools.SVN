using System.Collections.Generic;
using Terrasoft.Tools.SvnUI.Model.Enums;

namespace Terrasoft.Tools.SvnUI.Model.EventArgs {
	public class StartSvnOperationEventArgs : System.EventArgs {
		public SvnOperation SvnOperation { get; set; }
		public Dictionary<string, string> Arguments { get; set; }
	}
}
