using System;

namespace NullEngine.StateMachine
{
    class GameStateManager : StateManager
    {
        //static singleton reference
        public static GameStateManager man;

        //enable or disable debug state
        public static bool debugEnabled;
        
        public GameStateManager()
        {
            //singleton management
            if(man == null)
            {
                man = this;
            }
            else
            {
                Debug.Error("Singleton Failure @ GameStateManager");
            }

            if(System.IO.File.Exists("DEBUG_MODE_ENABLED"))
            {
                debugEnabled = true;
            }
        }

        public override void update()
        {
        }
    }
}
