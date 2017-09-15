using NullEngine.Component;
using NullEngine.Entity;
using System.Drawing;
using System;

namespace NullGame.Component
{
    class cBackgroundManager : iComponent
    {
        private Point displayedChunk;
        private bool lastInDungeon;

        public cBackgroundManager()
        {
            displayedChunk = Managers.WorldManager.man.currentChunkPos;
            lastInDungeon = Managers.WorldManager.man.currentChunk.inDungeon;
        }

        public void Run(renderable r)
        {
            if (displayedChunk != Managers.WorldManager.man.currentChunkPos || lastInDungeon != Managers.WorldManager.man.currentChunk.inDungeon)
            {
                r.tex = Managers.WorldManager.worldTex;
                displayedChunk = Managers.WorldManager.man.currentChunkPos;
                lastInDungeon = Managers.WorldManager.man.currentChunk.inDungeon;
            }
        }

        public void OnDestroy(renderable r)
        {
            //DO NOTHING
        }
    }
}
