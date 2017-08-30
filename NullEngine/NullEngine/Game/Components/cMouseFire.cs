using System.Collections.Generic;
using System.Drawing;
using NullEngine;
using NullEngine.Entity;
using NullEngine.Component;

namespace NullGame.Component
{
    public class cMouseFire : iComponent
    {
        List<cFireable> bullets;
        renderable pc;
        Point mousePos;
        Point playerPos;

        int currentBullet;

        public cMouseFire(renderable player)
        {
            pc = player;
            bullets = new List<cFireable>();
            currentBullet = 0;
        }

        public void addBullet(cFireable b)
        {
            bullets.Add(b);
        }

        public void Run(renderable r)
        {
            if(Game.input.isClickedRising(OpenTK.Input.MouseButton.Left))
            {
                mousePos = Game.input.mousePos;
                mousePos = Game.ScreenToWorldSpace(mousePos);
                playerPos = new Point((int)pc.pos.xPos, (int)pc.pos.yPos);

                bullets[currentBullet].Shoot(playerPos, mousePos);

                if (currentBullet >= bullets.Count - 1)
                {
                    currentBullet = 0;
                }
                else
                {
                    currentBullet++;
                }
            }
        }
    }
}
