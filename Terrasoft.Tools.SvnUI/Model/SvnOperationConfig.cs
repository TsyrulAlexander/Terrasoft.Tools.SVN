using System.Collections.Generic;

namespace Terrasoft.Tools.SvnUI.Model
{
	public class SvnOperationConfig {
		public SvnOperation SvnOperation { get; set; }
		public Dictionary<string, string> Arguments { get; set; }
	}
}
