using OpenTK;

namespace nullEngine.StateMachines
{
    //this is an abstract class to use as a base for the stateM machine manager
    abstract class StateManager
    {
        public iState CurrentState;
        public abstract void update(object sender, FrameEventArgs e);
    }
}
