using System;
using Terrasoft.Core;
using Terrasoft.Core.SVN;
using Console = System.Console;

namespace Terrasoft.Tools.SvnConsole
{
	internal class ConsoleLogger: ILogger
	{
		public void LogError(string warnMessage, string message = "") {
			ConsoleColor defaultColor = Console.ForegroundColor;
			Console.ForegroundColor = ConsoleColor.Red;
			Console.Write($@"{warnMessage} ");
			Console.ForegroundColor = defaultColor;
			Console.WriteLine(message);
		}

		public void LogInfo(string infoMessage, string message = "") {
			ConsoleColor defaultColor = Console.ForegroundColor;
			Console.ForegroundColor = ConsoleColor.DarkGreen;
			Console.Write($@"{infoMessage} ");
			Console.ForegroundColor = defaultColor;
			Console.WriteLine(message);
		}

		public void LogWarning(string infoMessage, string message = "") {
			ConsoleColor defaultColor = Console.ForegroundColor;
			Console.ForegroundColor = ConsoleColor.DarkYellow;
			Console.Write($@"{infoMessage} ");
			Console.ForegroundColor = defaultColor;
			Console.WriteLine(message);
		}
	}
}
