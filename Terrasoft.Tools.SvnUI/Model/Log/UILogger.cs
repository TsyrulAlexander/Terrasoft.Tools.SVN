using System;
using Terrasoft.Core;
using Terrasoft.Tools.SvnUI.Model.Enums;

namespace Terrasoft.Tools.SvnUI.Model.Log {
	public class UILogger : ILogger {
		public event Action<LogInfo> Execute;

		public void LogError(string warnMessage, string message = "") {
			OnExecute(new LogInfo {
				Message = $"{warnMessage} {message}", Level = LogLevel.Error
			});
		}

		public void LogInfo(string infoMessage, string message = "") {
			OnExecute(new LogInfo {
				Message = $"{infoMessage} {message}", Level = LogLevel.Information
			});
		}

		public void LogWarning(string infoMessage, string message = "") {
			OnExecute(new LogInfo {
				Message = $"{infoMessage} {message}", Level = LogLevel.Warning
			});
		}

		protected virtual void OnExecute(LogInfo obj) {
			Execute?.Invoke(obj);
		}
	}
}
