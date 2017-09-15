using NullEngine.Entity;
using NullEngine.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NullGame.Components
{
    class cBounce : cCollider
    {
        public int xVel;
        public int yVel;

        public List<cCollider> colliding;

        public cBounce(renderable r): base(r)
        {
            newVel();
        }

        public override void Run(renderable r)
        {
            if(!NullEngine.Game.input.KeyHeld(OpenTK.Input.Key.Space))
            {
                base.Run(r);

                colliding = NullEngine.Managers.CollisionManager.man.CheckCollision(this);

                if (colliding.Count > 0)
                {
                    if (colliding[0].rect.X > rect.X)
                    {
                        xVel = NullEngine.Game.rng.Next(-10, 0);
                    }

                    if (colliding[0].rect.X < rect.X)
                    {
                        xVel = NullEngine.Game.rng.Next(0, 10);
                    }

                    if (colliding[0].rect.Y > rect.Y)
                    {
                        yVel = NullEngine.Game.rng.Next(-10, 0);
                    }

                    if (colliding[0].rect.Y < rect.Y)
                    {
                        yVel = NullEngine.Game.rng.Next(0, 10);
                    }
                }

                r.setRelativePos(xVel, yVel);

                if (r.pos.xPos < NullEngine.Game.windowRect.Left)
                {
                    xVel = 5;
                }

                if (r.pos.xPos > NullEngine.Game.windowRect.Right)
                {
                    xVel = -5;
                }

                if (r.pos.yPos < NullEngine.Game.windowRect.Top)
                {
                    yVel = 5;
                }

                if (r.pos.yPos > NullEngine.Game.windowRect.Bottom)
                {
                    yVel = -5;
                }
            }
        }

        public void newVel()
        {
            xVel = NullEngine.Game.rng.Next(-10, 10);
            yVel = NullEngine.Game.rng.Next(-10, 10);
        }
    }
}
