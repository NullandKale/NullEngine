using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using nullEngine.Entity;

namespace nullEngine.Component
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

            colliding = Managers.CollisionManager.man.CheckCollision(this);

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
