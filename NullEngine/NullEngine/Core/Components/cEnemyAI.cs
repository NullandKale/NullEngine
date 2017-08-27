using System;
using nullEngine.Entity;
using System.Drawing;

namespace nullEngine.Component
{
    class cEnemyAI : iComponent
    {
        int speed;
        int viewDistance;
        cCollider collider;
        renderable target;

        Point targetPos;

        public cEnemyAI(int moveSpeed, cCollider c, renderable target, int dist)
        {
            speed = moveSpeed;
            viewDistance = dist;
            collider = c;
            this.target = target;
            targetPos = pickNewTargetLoc();
        }        

        public void Run(renderable r)
        {
            if(r.active)
            {
                Point move = moveTowardsTarget(targetPos);

                if (Math.Abs(target.pos.xPos - r.pos.xPos) < viewDistance && Math.Abs(target.pos.yPos - r.pos.yPos) < viewDistance)
                {
                    if (target.pos.xPos > r.pos.xPos)
                    {
                        move.X = speed;
                    }
                    if (target.pos.xPos < r.pos.xPos)
                    {
                        move.X = -speed;
                    }

                    if (target.pos.yPos < r.pos.yPos)
                    {
                        move.Y = -speed;
                    }
                    if (target.pos.yPos > r.pos.yPos)
                    {
                        move.Y = speed;
                    }
                }

                Point p = Managers.CollisionManager.WillItCollide(collider, move.X, move.Y);
                r.setRelativePos(p);
            }
        }

        Point moveTowardsTarget(Point p)
        {
            int moveX = 0;
            int moveY = 0;

            if(p.X > collider.rect.X)
            {
                moveX = speed;
            }
            if(p.X < collider.rect.X)
            {
                moveX = -speed;
            }
            if (p.Y > collider.rect.Y)
            {
                moveY = speed;
            }
            if (p.X < collider.rect.X)
            {
                moveY = -speed;
            }

            if(Math.Abs(p.X - collider.rect.X) < 20 && Math.Abs(p.Y - collider.rect.Y) < 20)
            {
                targetPos = pickNewTargetLoc();
            }

            return new Point(moveX, moveY);
        }

        Point pickNewTargetLoc()
        {
            return new Point(Game.rng.Next(50, Game.worldMaxX - 50), Game.rng.Next(50, Game.worldMaxY - 50));
        }
    }
}
