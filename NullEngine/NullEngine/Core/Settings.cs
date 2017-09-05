using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace NullEngine
{
    public static class Settings
    {
        public static int xRes = 1600;
        public static int yRes = 900;
        public static int colorDepth = 32;
        public static int bitDepth = 8;
        public static int updateSpeed = 60;


        //THIS IS BAD AND I SHOULD FEEL BAD (Alec)
        public static void Load(String fileName)
        {
            string[] text = File.ReadAllLines(fileName);

            xRes = int.Parse(text[0]);
            yRes = int.Parse(text[1]);
            colorDepth = int.Parse(text[2]);
            bitDepth = int.Parse(text[3]);

        }


        //SO IS THIS (Alec)
        public static void Save(String fileName)
        {
            string[] text = new string[4];

            text[0] = xRes.ToString();
            text[1] = yRes.ToString();
            text[2] = colorDepth.ToString();
            text[3] = bitDepth.ToString();

            File.AppendAllLines(fileName, text);
        }
    }
}
