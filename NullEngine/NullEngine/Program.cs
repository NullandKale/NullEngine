using System;

namespace nullEngine
{
    class Program
    {
      static void Main(string[] args)
        {
            //entry point
            Console.WriteLine("Loading.......");

            //create a window and set graphics mode
            OpenTK.GameWindow window = new OpenTK.GameWindow(1600, 900, new OpenTK.Graphics.GraphicsMode(32,8,0,0));
            //create a game singleton
            Game game = new Game(window);
            //set window run speed
            window.Run(1.0 / 60.0);
        }

        //public exit function
        public static void exit()
        {
            Environment.Exit(0);
        }
    }
}
