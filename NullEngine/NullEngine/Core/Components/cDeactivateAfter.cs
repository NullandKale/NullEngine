using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nullEngine.Entity___Component
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
