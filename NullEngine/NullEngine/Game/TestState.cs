using System;
using System.Collections.Generic;
using nullEngine.Entity;
using nullEngine.Component;

namespace NullEngine.Game
{
    class TestState
    {
        public quad pc;

        private List<Action> updaters;

        public TestState()
        {
            updaters = new List<Action>();
            pc = new quad("Game/Content/roguelikeCharBeard_transparent.png");

            updaters.Add(pc.update);
        }

        public void update()
        {
            foreach (Action update in updaters)
            {
                update.Invoke();
            }
        }
    }
}
