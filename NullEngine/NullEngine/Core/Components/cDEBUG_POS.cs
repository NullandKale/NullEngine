﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using nullEngine.Entity;

namespace nullEngine.Component
{
    class cDEBUG_POS : iComponent
    {
        public void Run(renderable r)
        {
            if(Game.input.KeyFallingEdge(OpenTK.Input.Key.R))
            {
                Console.WriteLine("[" + r.pos.xPos + "," + r.pos.yPos + "]");
            }
        }
    }
}
