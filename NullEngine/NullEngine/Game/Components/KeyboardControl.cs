
using NullEngine;
using NullEngine.Component;
using NullEngine.Entity;
using OpenTK.Input;

namespace NullGame.Component
{
    class KeyboardControl : iComponent
    {
        protected int speed;
        protected bool xPositiveMove;
        protected bool xNegativeMove;
        protected bool yPositiveMove;
        protected bool yNegativeMove;


        public KeyboardControl(int speed)
        {
            this.speed = speed;

            xPositiveMove = true;
            xNegativeMove = true;

            yPositiveMove = true;
            yNegativeMove = true;
        }

        public void CollidingOn(int x, int y)
        {
            if(x > 0)
            {
                xPositiveMove = false;
            }
            if(x < 0)
            {
                xNegativeMove = false;
            }
            if(x == 0)
            {
                xPositiveMove = true;
                xNegativeMove = true;
            }

            if(y > 0)
            {
                yPositiveMove = false;
            }
            if(y < 0)
            {
                yNegativeMove = false;
            }
            if(y == 0)
            {
                yPositiveMove = true;
                yNegativeMove = true;
            }
        }

        public virtual void Run(renderable r)
        {
            if(Game.input.KeyHeld(Key.W) && yNegativeMove)
            {
                r.pos.yPos -= speed;
            }

            if (Game.input.KeyHeld(Key.S) && yPositiveMove)
            {
                r.pos.yPos += speed;
            }

            if (Game.input.KeyHeld(Key.A) && xNegativeMove)
            {
                r.pos.xPos -= speed;
            }

            if (Game.input.KeyHeld(Key.D) && xPositiveMove)
            {
                r.pos.xPos += speed;
            }
        }
    }
}
