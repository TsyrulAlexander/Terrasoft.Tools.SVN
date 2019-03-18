using System;

namespace Terrasoft.Core.SVN
{
    internal static class Logger
    {
        internal static void LogError(string warnMessage, string message = "") {
            ConsoleColor defaultColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write($@"{warnMessage} ");
            Console.ForegroundColor = defaultColor;
            Console.WriteLine(message);
        }

        internal static void LogInfo(string infoMessage, string message = "") {
            ConsoleColor defaultColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write($@"{infoMessage} ");
            Console.ForegroundColor = defaultColor;
            Console.WriteLine(message);
        }

        internal static void LogWarning(string infoMessage, string message = "") {
            ConsoleColor defaultColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write($@"{infoMessage} ");
            Console.ForegroundColor = defaultColor;
            Console.WriteLine(message);
        }
    }
}