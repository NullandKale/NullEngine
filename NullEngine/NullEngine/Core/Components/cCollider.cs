﻿using NullEngine.Entity;
using System.Drawing;

namespace NullEngine.Component
{
    public class cCollider : iComponent
    {
        public renderable rRef;
        public Rectangle rect;
        public Point key;

        public StateMachine.iState parentState;

        public bool component;

        public cCollider(renderable r)
        {
            Managers.CollisionManager.addCollider(this);
            rRef = r;
            parentState = r.parentState;
            rect = new Rectangle((int)rRef.pos.xPos, (int)rRef.pos.yPos, rRef.getWidth(), rRef.getHeight());
            component = true;
        }

        public cCollider(Rectangle r, StateMachine.iState parent)
        {
            parentState = parent;
            rect = r;
            Managers.CollisionManager.addCollider(this);
            Managers.CollisionManager.moveCollider(this);
            component = false;
        }

        ~cCollider()
        {
            Managers.CollisionManager.removeCollider(this);
        }

        public virtual void Run(renderable r)
        {
            if(r.active && component)
            {
                if ((int)r.pos.xPos != rect.X || (int)r.pos.yPos != rect.Y)
                {
                    rect.X = (int)r.pos.xPos;
                    rect.Y = (int)r.pos.yPos;
                    Managers.CollisionManager.moveCollider(this);
                }
            }
        }

        public void OnDestroy(renderable r)
        {
            Managers.CollisionManager.removeCollider(this);
        }

        public bool collides(cCollider c1)
        {
            if(component)
            {
                if (this.rRef == null || c1.rRef == null || c1 == this || !c1.rRef.active || !rRef.active)
                {
                    return false;
                }
            }
            else
            {
                if(c1 == this)
                {
                    return false;
                }
            }

            return rect.IntersectsWith(c1.rect);
        }

        public bool collides(Rectangle otherRect, cCollider c1)
        {
            if (component)
            {
                if (this.rect == otherRect || !rRef.active || !c1.rRef.active)
                {
                    return false;
                }
            }
            else
            {
                if(c1 == this)
                {
                    return false;
                }
            }

            return rect.IntersectsWith(otherRect);
        }
    }
}
