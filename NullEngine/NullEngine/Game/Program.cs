using NullEngine;
using NullGame.StateMachine;
using NullEngine.Core;

namespace NullGame
{
    class Program
    {
        static void Run()
        {
            // Runs the game.
            Game game = new Game();

            //create game logic statemachine
            GameStateManager gMan = new GameStateManager();

            //give the game the current update function
            Game.toUpdate.Add(gMan.update);

            //Comment line 21 an line 18 and unComment the below line to activate collision Test
            //Game.currentState = new TestState();

            //RUN THE GAME!!!!
            Game.Run();
        }

        static void Main(string[] args)
        {
            Settings.Initialize();

            Run();
        }
    }
}
