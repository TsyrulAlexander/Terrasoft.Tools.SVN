using System.Collections.Generic;
using Terrasoft.Tools.SvnUI.Model.Enums;

namespace Terrasoft.Tools.SvnUI.Model.EventArgs
{
	public class StartConfigurationOperationEventArgs {
		public ConfigurationOperation Operation { get; set; }
		public Dictionary<string, string> Args { get; set; }
	}
}
