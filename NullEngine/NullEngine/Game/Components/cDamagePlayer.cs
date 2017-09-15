using System.Collections.Generic;
using NullEngine.Entity;
using NullEngine.Component;
using NullEngine.Managers;

namespace NullGame.Component
{
    class cDamagePlayer : iComponent
    {
        renderable pc;
        cHealth pcHealth;
        int damage;
        cCollider collider;
        List<cCollider> collidingWith;

        public cDamagePlayer(renderable player,cHealth playerHealth, int damageAmount, cCollider col)
        {
            pc = player;
            pcHealth = playerHealth;
            damage = damageAmount;
            collider = col;
        }

        public void Run(renderable r)
        {
            if(r.active)
            {
                collidingWith = CollisionManager.man.CheckCollision(collider, 5);

                for (int i = 0; i < collidingWith.Count; i++)
                {
                    if (collidingWith[i].rRef == pc)
                    {
                        pcHealth.damage(damage);
                    }
                }
            }
        }

        public void OnDestroy(renderable r)
        {
            //DO NOTHING
        }
    }
}
