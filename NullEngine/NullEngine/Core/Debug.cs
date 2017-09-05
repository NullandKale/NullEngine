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

        private static bool toFile = false;
        private static bool annotate = false;

        /// <summary>
        /// A bool to test if the debug statement should output to a file.
        /// </summary>
        public static bool OutputLog
        {
            get
            {
                return toFile;
            }
            set
            {
                toFile = value;
            }
        }

        public static bool Annotate
        {
            get
            {
                return annotate;
            }
            set
            {
                annotate = value;
            }
        }

        /// <summary>
        /// Outputs a line of text to the console.
        /// </summary>
        /// <param name="text">The text to output</param>
        public static void Text(string text)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine((annotate ? "Text: " :  string.Empty) + text);
            Console.ForegroundColor = ConsoleColor.White;

            if (toFile)
            {
                ToLogFile("Text: " + text);
            }
        }

        /// <summary>
        /// Outputs a warning to the console.
        /// </summary>
        /// <param name="text">The text to output</param>
        public static void Warning(string text)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine((annotate ? "Warning: " : string.Empty) + text);
            Console.ForegroundColor = ConsoleColor.White;

            if (toFile)
            {
                ToLogFile("Warning: " + text);
            }
        }

        /// <summary>
        /// Outputs an error to the console.
        /// </summary>
        /// <param name="text">The text to output</param>
        public static void Error(string text)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine((annotate ? "Error: " : string.Empty) + text);
            Console.ForegroundColor = ConsoleColor.White;

            if (toFile)
            {
                ToLogFile("Error: " + text);
            }
        }

        /// <summary>
        /// Outputs a line of text to the log.
        /// </summary>
        /// <param name="text">The text to log.</param>
        private static void ToLogFile(string text)
        {
            logLines.Add(text);
            File.WriteAllLines(Directory.GetCurrentDirectory() + "Log.txt", logLines.ToArray());
        }
    }
}
