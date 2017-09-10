using System;
using System.IO;
using System.Collections.Generic;
using System.Reflection;

namespace NullEngine
{
    /// <summary>
    /// A class that handles the settings of the engine.
    /// </summary>
    public static class Settings
    {
        public static int xRes = 1600;
        public static int yRes = 900;
        public static int colorDepth = 32;
        public static int bitDepth = 8;
        public static int updateRate = 60;

        /// <summary>
        /// Creates the settings file.
        /// </summary>
        /// <param name="fileName">The name of the file.</param>
        private static void CreateSettingsFile(String fileName)
        {
            File.Create(fileName).Dispose();
        }

        /// <summary>
        /// Loads the settings file.
        /// </summary>
        /// <param name="fileName">The name of the settings file.</param>
        public static void Load(String fileName)
        {
            if (File.Exists(fileName))
            {
                // Reads the lines from the file.
                string[] text = File.ReadAllLines(fileName);

                // Loads the settings.
                for (var i = 0; i < text.Length; i++)
                {
                    var split = text[i].Split(' ');
                    switch (split[0])
                    {
                        case "XRes":
                            xRes = int.Parse(split[1]);
                            break;
                        case "YRes":
                            yRes = int.Parse(split[1]);
                            break;
                        case "ColorDepth":
                            colorDepth = int.Parse(split[1]);
                            break;
                        case "BitDepth":
                            bitDepth = int.Parse(split[1]);
                            break;
                        case "UpdateRate":
                            updateRate = int.Parse(split[1]);
                            break;
                    }
                }
            }
            else
            {
                // Creates a new settings file with the default values.
                Save(fileName);
            }
        }

        /// <summary>
        /// Saves the settings to the settings file.
        /// </summary>
        /// <param name="fileName">The name of the settings file.</param>
        public static void Save(String fileName)
        {
            // Checks if a settings file exists.
            if (!File.Exists(fileName))
            {
                CreateSettingsFile(fileName);
            }

            // Creates the settings file's contents.
            var text = new List<string>
            {
                "XRes " + xRes.ToString(),
                "YRes " + yRes.ToString(),
                "ColorDepth " + colorDepth.ToString(),
                "BitDepth " + bitDepth.ToString(),
                "UpdateRate " + updateRate.ToString()
            };
            File.AppendAllLines(fileName, text.ToArray());
        }

        /// <summary>
        /// Initializes the required Assemblies to run the program.
        /// </summary>
        public static void Initialize()
        {
            AppDomain.CurrentDomain.AssemblyResolve += (Object sender, ResolveEventArgs arg) =>
            {
                String thisExe = Assembly.GetExecutingAssembly().GetName().Name;
                AssemblyName embeddedAssembly = new AssemblyName(arg.Name);
                String resourceName = thisExe + "." + embeddedAssembly.Name + ".dll";

                using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
                {
                    Byte[] assemblyData = new Byte[stream.Length];
                    stream.Read(assemblyData, 0, assemblyData.Length);
                    return Assembly.Load(assemblyData);
                }
            };
        }
    }
}
