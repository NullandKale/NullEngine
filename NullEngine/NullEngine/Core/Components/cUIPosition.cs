﻿using System.Drawing;
using nullEngine.Entity;

namespace nullEngine.Component
{
    class cUIPosition : iComponent
    {
        text uiEntity;
        Point lastPos;
        Point currentPos;

        public cUIPosition(text ui)
        {
            uiEntity = ui;
        }

        public void Run(renderable r)
        {
            currentPos = GetPlayerPos();
            if (lastPos != currentPos)
            {
                uiEntity.ChangeText("[" + Managers.WorldManager.man.currentChunkPos.X + "," + Managers.WorldManager.man.currentChunkPos.Y + "] {" + currentPos.X + "," + currentPos.Y + "}");
            }
            lastPos = currentPos;
        }

        private Point GetPlayerPos()
        {
            return new Point((int)StateMachines.GameStateManager.man.gState.playerCharacter.pos.xPos / 64, (int)StateMachines.GameStateManager.man.gState.playerCharacter.pos.yPos / 64);
        }
    }
}
