using System;
using Terrasoft.Core.SVN;

namespace Terrasoft.Tools.SvnConsole
{
	class ConsoleLogger: ILogger
	{
		public void LogError(string warnMessage, string message = "") {
			ConsoleColor defaultColor = System.Console.ForegroundColor;
			System.Console.ForegroundColor = ConsoleColor.Red;
			System.Console.Write($@"{warnMessage} ");
			System.Console.ForegroundColor = defaultColor;
			System.Console.WriteLine(message);
		}

		public void LogInfo(string infoMessage, string message = "") {
			ConsoleColor defaultColor = System.Console.ForegroundColor;
			System.Console.ForegroundColor = ConsoleColor.DarkGreen;
			System.Console.Write($@"{infoMessage} ");
			System.Console.ForegroundColor = defaultColor;
			System.Console.WriteLine(message);
		}

		public void LogWarning(string infoMessage, string message = "") {
			ConsoleColor defaultColor = System.Console.ForegroundColor;
			System.Console.ForegroundColor = ConsoleColor.DarkYellow;
			System.Console.Write($@"{infoMessage} ");
			System.Console.ForegroundColor = defaultColor;
			System.Console.WriteLine(message);
		}
	}
}
