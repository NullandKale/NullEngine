namespace NullEngine
{
    using System;
    using System.IO;
    using System.Collections.Generic;

    /// <summary>
    /// Handles logging Debug messages to the console.
    /// </summary>
    public static class Debug
    {
        private static List<string> logLines = new List<string>();

        private static bool toFile = true;

        public static bool OutputLog
        {
            get
            {
                return toFile;
            }
            set
            {
                toFile = OutputLog;
            }
        }

        public static void Text(string text)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Text: " + text);
            Console.ForegroundColor = ConsoleColor.White;

            if (toFile)
            {
                ToLogFile("Text: " + text);
            }
        }

        public static void Warning(string text)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Warning: " + text);
            Console.ForegroundColor = ConsoleColor.White;

            if (toFile)
            {
                ToLogFile("Warning: " + text);
            }
        }

        public static void Error(string text)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Error: " + text);
            Console.ForegroundColor = ConsoleColor.White;

            if (toFile)
            {
                ToLogFile("Error: " + text);
            }
        }

        private static void ToLogFile(string text)
        {
            logLines.Add(text);
            File.WriteAllLines(Directory.GetCurrentDirectory() + "Log.txt", logLines.ToArray());
        }
    }
}
