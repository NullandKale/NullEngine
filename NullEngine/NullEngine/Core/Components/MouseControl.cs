using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using nullEngine.Entity;

namespace nullEngine.Component
{
    class MouseControl : iComponent
    {
        public void Run(renderable r)
        {
            r.pos.xPos = Game.input.mousePos.X;
            r.pos.yPos = Game.input.mousePos.Y;
        }
    }
}
