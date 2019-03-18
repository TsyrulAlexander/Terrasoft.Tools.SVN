using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Terrasoft.Core.SVN
{
	public interface ILogger {
		void LogError(string warnMessage, string message = "");
		void LogInfo(string infoMessage, string message = "");
		void LogWarning(string infoMessage, string message = "");
	}
}
