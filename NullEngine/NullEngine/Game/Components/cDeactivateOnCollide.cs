using System.Collections.Generic;
using NullEngine.Entity;
using NullEngine.Component;
using NullEngine;
using NullEngine.Managers;

namespace NullGame.Component
{
    class cDeactivateOnCollide : cCollider
    {
        List<cCollider> colliding;
        renderable PC;

        public cDeactivateOnCollide(renderable r, renderable player) : base (r)
        {
            PC = player;
        }

        public override void Run(renderable r)
        {
            base.Run(r);

            colliding = CollisionManager.man.CheckCollision(this);

            for (int i = 0; i < colliding.Count; i++)
            {
                if (colliding[i].rRef != PC)
                {
                    colliding[i].rRef.active = false;
                    r.active = false;
                    break;
                }
            }
        }
    }
}
