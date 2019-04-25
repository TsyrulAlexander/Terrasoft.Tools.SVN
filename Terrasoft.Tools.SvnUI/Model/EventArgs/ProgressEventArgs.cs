using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Terrasoft.Tools.SvnUI.Model.EventArgs
{
	public class ProgressEventArgs : System.EventArgs {
		public bool InProgress { get; set; }
		public object Owner { get; set; }
	}
}
