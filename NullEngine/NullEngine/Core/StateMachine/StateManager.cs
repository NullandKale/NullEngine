
namespace NullEngine.StateMachine
{

    /// <summary>
    /// this is an abstract class to use as a base for the stateM machine manager...
    /// </summary>
    abstract class StateManager
    {
        public iState CurrentState;
        public abstract void update();
    }
}
