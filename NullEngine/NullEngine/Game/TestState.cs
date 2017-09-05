using System;
using System.Collections.Generic;
using NullEngine.Entity;

namespace NullGame
{
    class TestState : NullEngine.StateMachine.iState
    {
        public quad pc;
        public NullEngine.Button b;

        private List<Action> updaters;

        public TestState()
        {
            updaters = new List<Action>();
            pc = new quad("Game/Content/roguelikeCharBeard_transparent.png", this);
            updaters.Add(pc.update);

            b = new NullEngine.Button("Hello!", NullEngine.Game.buttonBackground, onButton, OpenTK.Input.MouseButton.Left, this);
            updaters.Add(b.update);
        }

        public void update()
        {
            foreach (Action update in updaters)
            {
                update.Invoke();
            }
        }

        public void onButton()
        {
            Console.WriteLine("Hello World!");
        }

        public void addUpdater(Action toAdd)
        {
            updaters.Add(toAdd);
        }

        public void enter()
        {
            throw new NotImplementedException();
        }
    }
}
