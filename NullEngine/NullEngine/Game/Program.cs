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

            //create a game singleton
            Game game = new Game();

            //create game logic statemachine
            StateMachine.GameStateManager gMan = new StateMachine.GameStateManager();

            //give the game the current update function
            Game.toUpdate.Add(gMan.update);

            //RUN THE GAME!!!!
            Game.Run();
        }
    }
}
