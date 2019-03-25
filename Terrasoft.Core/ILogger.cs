namespace Terrasoft.Core
{
	public interface ILogger {
		void LogError(string warnMessage, string message = "");
		void LogInfo(string infoMessage, string message = "");
		void LogWarning(string infoMessage, string message = "");
	}
}
