using System;
using nullEngine.Entity;
using System.Drawing;

namespace nullEngine.Component
{
    public class cFireable : iComponent
    {
        renderable bullet;
        bool isFired;
        float step;
        float stepAmount;
        float speed;

        Point start;
        Point end;

        public cFireable(renderable r, float speed)
        {
            isFired = false;
            step = 0;
            stepAmount = 0;

            this.speed = speed;
            bullet = r;
        }

        public void Run(renderable r)
        {
            if(isFired)
            {
                float[] move = lerp(end.X, start.X, end.Y, start.Y);

                bullet.setPos((int)move[0], (int)move[1]);

                if(step > 3)
                {
                    step = 0;
                    bullet.active = false;
                    isFired = false;
                }
            }
        }

        public void Shoot(Point s, Point e)
        {
            isFired = true;
            bullet.active = true;
            start = s;
            end = e;

            float hypo = dist(start.X, end.X, start.Y, end.Y);
            stepAmount = speed / hypo;
        }

        float[] lerp(float u1, float u0, float v1, float v0)
        {
            float[] temp = new float[2];
            step += stepAmount;
            temp[0] = ((1 - step) * u0) + step * u1;
            temp[1] = ((1 - step) * v0) + step * v1;
            return temp;
        }

        float dist(float x1, float x2, float y1, float y2)
        {
            float dx = Math.Abs(x1 - x2);
            float dy = Math.Abs(y1 - y2);
            return (float)Math.Sqrt(Math.Pow(dx, 2) + Math.Pow(dy, 2));
        }
    }
}
