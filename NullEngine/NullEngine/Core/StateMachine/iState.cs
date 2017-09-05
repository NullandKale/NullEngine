namespace NullEngine.StateMachine
{
    //this is an interface for the basic state for the state machines
    public interface iState
    {
        void enter();
        void update();
        void addUpdater(System.Action toAdd);
    }
}
