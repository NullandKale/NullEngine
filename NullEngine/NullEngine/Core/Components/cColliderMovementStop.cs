﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nullEngine.Entity___Component
{
    class cColliderMovementStop : cCollider
    {
        private KeyboardControl keyboard;
        int xCol;
        int yCol;

        public cColliderMovementStop(renderable r, KeyboardControl k) : base(r)
        {
            keyboard = k;
            xCol = 0;
            yCol = 0;
        }

        public override void Run(renderable r)
        {
            base.Run(r);
            keyboard.CollidingOn(xCol, yCol);
            xCol = 0;
            yCol = 0;
        }
    }
}
