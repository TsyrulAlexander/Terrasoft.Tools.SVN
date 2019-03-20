using System;
using Terrasoft.Core.SVN;
using Terrasoft.Tools.SvnUI.Model;

namespace Terrasoft.Tools.SvnUI {
	public class UILogger : ILogger {
		public event Action<LogInfo> Execute;

		public void LogError(string warnMessage, string message = "") {
			OnExecute(new LogInfo {
				Message = message, Level = LogLevel.Error
			});
		}

		public void LogInfo(string infoMessage, string message = "") {
			OnExecute(new LogInfo {
				Message = message, Level = LogLevel.Information
			});
		}

		public void LogWarning(string infoMessage, string message = "") {
			OnExecute(new LogInfo {
				Message = message, Level = LogLevel.Warning
			});
		}

		protected virtual void OnExecute(LogInfo obj) {
			Execute?.Invoke(obj);
		}
	}
}
