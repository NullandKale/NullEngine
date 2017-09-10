using NullEngine;
using NullGame.StateMachine;
using System;

namespace NullGame
{
    class Program
    {
        static void Main(string[] args)
        {
            //entry point
            Debug.Text("Loading");

            //create a game singleton
            Game game = new Game();

            //create game logic statemachine
            GameStateManager gMan = new GameStateManager();

            //give the game the current update function
            Game.toUpdate.Add(gMan.update);

            //Comment line 20 an line 17 and unComment the below line to activate collision Test
            //Game.currentState = new TestState();

            //RUN THE GAME!!!!
            Game.Run();
        }
    }
}
