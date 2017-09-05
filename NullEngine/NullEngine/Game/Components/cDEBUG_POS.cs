using System;
using NullEngine;
using NullEngine.Entity;
using NullEngine.Component;

namespace NullGame.Component
{
    class cDEBUG_POS : iComponent
    {
        public void Run(renderable r)
        {
            if(Game.input.KeyFallingEdge(OpenTK.Input.Key.R))
            {
                Debug.Text("[" + r.pos.xPos + "," + r.pos.yPos + "]");
            }
        }
    }
}
