namespace NullEngine.StateMachine
{
    //this is an interface for the basic state for the state machines
    interface iState
    {
        void enter();
        void update();
    }
}
