using NullEngine.Component;
using NullEngine.Entity;
using System.Drawing;

namespace NullGame.Component
{
    class cBackgroundManger : iComponent
    {
        private Point displayedChunk;
        private bool lastInDungeon;

        public cBackgroundManger()
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
    }
}
