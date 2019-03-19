using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Terrasoft.Tools.SvnUI.Model
{
	public class SvnOperationConfig
	{
		public SvnOperation SvnOperation { get; set; }
		public Dictionary<string, string> Arguments { get; set; }
	}
}
