using System;
using NullEngine;

namespace NullGame.StateMachine
{
    class GameStateManager : NullEngine.StateMachine.GameStateManager
    {
        public static new GameStateManager man;

        //state storage
        public GameState gState;
        public MenuState mState;
        public PauseState pState;
        public DebugState dState;

        public GameStateManager() : base()
        {
            if (man == null)
            {
                man = this;
            }

            //create each state
            mState = new MenuState();
            pState = new PauseState();
            gState = new GameState();

            if(debugEnabled)
            {
                dState = new DebugState();
                Console.WriteLine("Debug is enabled, loading debugState");
            }
        }

        public override void update()
        {
            //on startup set current state to menuState and update current state
            if(CurrentState == null)
            {
                CurrentState = mState;
                mState.enter();
                Game.currentState = CurrentState;
            }
            else
            {
                //update current state
                Game.currentState = CurrentState;
            }

            if(debugEnabled)
            {
                if (Game.input.KeyFallingEdge(OpenTK.Input.Key.Tilde) && CurrentState != dState)
                {
                    dState.previousState = CurrentState;
                    CurrentState = dState;
                    dState.enter();
                }
            }
        }
    }
}
