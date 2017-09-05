using NullEngine;
using System;

namespace NullGame
{
    class Program
    {
        static void Main(string[] args)
        {
            //entry point
            Debug.Text("Loading");

            //create a window and set graphics mode
            OpenTK.GameWindow window = new OpenTK.GameWindow(1600, 900, new OpenTK.Graphics.GraphicsMode(32, 8, 0, 0));

            //create a game singleton
            Game game = new Game(window);

            //create game logic statemachine
            StateMachine.GameStateManager gMan = new StateMachine.GameStateManager();

            //give the game the current update function
            Game.toUpdate.Add(gMan.update);

            //Test State
            //Game.currentState = new TestState();

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
