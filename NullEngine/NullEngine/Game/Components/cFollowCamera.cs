using NullEngine;
using NullEngine.Component;
using NullEngine.Entity;

namespace NullGame.Component
{
    class cFollowCamera : iComponent
    {
        renderable followTarget;

        public cFollowCamera(renderable target)
        {
            followTarget = target;
        }


        public void Run(renderable r)
        {
            int halfWindowWidth = Game.windowRect.Width / 2;
            int halfWindowHeight = Game.windowRect.Height / 2;

            int moveX = 0;
            int moveY = 0;

            moveX = (int)followTarget.pos.xPos;
            moveY = (int)followTarget.pos.yPos;

            if (followTarget.pos.xPos <= halfWindowWidth)
            {
                moveX = halfWindowWidth;
            }
            if (followTarget.pos.xPos >= Game.worldRect.Width - halfWindowWidth)
            {
                moveX = Game.worldRect.Width - halfWindowWidth;
            }

            if (followTarget.pos.yPos <= halfWindowHeight)
            {
                moveY = halfWindowHeight;
            }
            if (followTarget.pos.yPos >= Game.worldRect.Height - halfWindowHeight)
            {
                moveY = Game.worldRect.Height - halfWindowHeight;
            }

            Game.SetWindowCenterOffset(moveX, moveY);
        }

        public void OnDestroy(renderable r)
        {
            //DO NOTHING
        }
    }
}
