using System;

namespace Terrasoft.Tools.SVN
{
    internal static class Logger
    {
        internal static void LogError(string warnMessage, string message = "") {
            var defaultColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write($@"{warnMessage} ");
            Console.ForegroundColor = defaultColor;
            Console.WriteLine(message);
        }
        internal static void LogInfo(string infoMessage, string message = "") {
            var defaultColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write($@"{infoMessage} ");
            Console.ForegroundColor = defaultColor;
            Console.WriteLine(message);
        }
    }
}