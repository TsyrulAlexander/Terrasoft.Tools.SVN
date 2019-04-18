using System;

namespace Terrasoft.Tools.Svn
{
    internal static class Logger
    {
        internal static void Error(string errorMessage, string message = "")
        {
            ConsoleColor defaultColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write($@"{errorMessage}	");
            Console.ForegroundColor = defaultColor;
            Console.WriteLine(message);
        }

        internal static void Info(string infoMessage, string message = "")
        {
            ConsoleColor defaultColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write($@"{infoMessage}	");
            Console.ForegroundColor = defaultColor;
            Console.WriteLine(message);
        }

        internal static void Warning(string infoMessage, string message = "")
        {
            ConsoleColor defaultColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write($@"{infoMessage}	");
            Console.ForegroundColor = defaultColor;
            Console.WriteLine(message);
        }
    }
}