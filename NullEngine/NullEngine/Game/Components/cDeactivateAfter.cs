using NullEngine;
using NullEngine.Component;
using NullEngine.Entity;

namespace NullGame.Component
{
    class cDeactivateAfter : iComponent
    {
        float timer;
        float maxTime;

        public cDeactivateAfter(float lifetime)
        {
            maxTime = lifetime;
            timer = 0;
        }

        public void Run(renderable r)
        {
            if(r.active)
            {
                if(timer >= maxTime)
                {
                    timer = 0;
                    r.active = false;
                }
                else
                {
                    timer += Game.frameTime;
                }
            }
            else
            {
                timer = 0;
            }
        }
    }
}
