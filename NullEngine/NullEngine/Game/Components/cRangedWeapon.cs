using System;
using System.Collections.Generic;
using NullEngine.Entity;
using NullEngine.Component;

namespace NullGame.Component
{
    class cRangedWeapon : iComponent
    {
        public quad[] bullets;

        public List<Action> updaters;

        public cRangedWeapon(renderable playerCharacter, cMouseFire playerBulletMan, int bulletCount)
        {
            updaters = new List<Action>();

            bullets = new quad[bulletCount];
            for (int i = 0; i < bullets.Length; i++)
            {
                bullets[i] = new quad("Game/Content/bullet.png");
                bullets[i].active = false;
                bullets[i].AddComponent(new cDeactivateOnCollide(bullets[i], playerCharacter));
                bullets[i].AddComponent(new cDeactivateAfter(10000));
                cFireable bulletFireable = new cFireable(bullets[i], 20);
                bullets[i].AddComponent(bulletFireable);
                playerBulletMan.addBullet(bulletFireable);
                updaters.Add(bullets[i].update);
            }
        }

        public void Run(renderable r)
        {
            for(int i = 0; i < updaters.Count; i++)
            {
                updaters[i].Invoke();
            }
        }
    }
}
