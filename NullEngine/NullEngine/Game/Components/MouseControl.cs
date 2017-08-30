using NullEngine;
using NullEngine.Component;
using NullEngine.Entity;

namespace NullGame.Component
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
