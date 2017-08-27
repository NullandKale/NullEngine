using System;
using OpenTK;

namespace nullEngine.StateMachines
{
    class GameStateManager : StateManager
    {
        //static singleton reference
        public static GameStateManager man;

        //enable or disable debug state
        public static bool debugEnabled;

        //state storage
        public GameState gState;
        public MenuState mState;
        public PauseState pState;
        public DebugState dState;

        public GameStateManager()
        {
            //singleton management
            if(man == null)
            {
                man = this;
            }
            else
            {
                Console.WriteLine("Singleton Failure @ GameStateManager");
            }

            if(System.IO.File.Exists("DEBUG_MODE_ENABLED"))
            {
                debugEnabled = true;
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

            //add update function to update call list
            Game.window.UpdateFrame += update;
        }

        public override void update(object sender, FrameEventArgs e)
        {
            //on startup set current state to menuState and update current state
            if(CurrentState == null)
            {
                CurrentState = mState;
                mState.enter();
            }
            else
            {
                //update current state
                CurrentState.update();
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
