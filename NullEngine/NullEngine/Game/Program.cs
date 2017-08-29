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

            //create game logic statemachine
            StateMachines.GameStateManager gMan = new StateMachines.GameStateManager();

            //give the game the current update function
            game.currentState = gMan.update;

            //Test State
            //NullEngine.Game.TestState t = new NullEngine.Game.TestState();
            //game.currentState = t.update;

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
