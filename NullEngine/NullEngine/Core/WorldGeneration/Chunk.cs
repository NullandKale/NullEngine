using NullGame.Managers;
using System;
using System.Drawing;

namespace NullEngine.WorldGen
{
    [Serializable]
    public class Chunk
    {
        public worldTile[,] backgroundTiles;
        public worldTile[,] foregroundTiles;
        public Point key;
        public Rectangle chunkRect;

        public bool hasDungeon;
        public bool inDungeon;
        public Chunk dungeon;

        public WorldData wData;

        private Bitmap backgroundBitmap;
        private Bitmap dbackgroundBitmap;
        private bool textureGenerated;
        private bool dtextureGenerated;
        private bool textureOld;
        private int size;
        private bool regen;

        [NonSerialized]
        private Texture2D backgroundTexture;

        public Chunk(int xSize, int xCord, int yCord, WorldData data)
        {
            key = new Point(xCord, yCord);
            backgroundTiles = new worldTile[xSize, xSize];
            foregroundTiles = new worldTile[xSize, xSize];
            size = xSize;
            textureGenerated = false;
            dtextureGenerated = false;
            regen = false;
            textureOld = false;
            chunkRect = new Rectangle(xCord * xSize, yCord * xSize, xSize, xSize);
            wData = data;
            inDungeon = false;
        }

        public Chunk(int xSize, int xCord, int yCord)
        {
            key = new Point(xCord, yCord);
            backgroundTiles = new worldTile[xSize, xSize];
            foregroundTiles = new worldTile[xSize, xSize];
            size = xSize;
            textureGenerated = false;
            dtextureGenerated = false;
            regen = false;
            textureOld = false;
            chunkRect = new Rectangle(xCord * xSize, yCord * xSize, xSize, xSize);
            inDungeon = false;
        }


        public Texture2D getBackgroundTexture()
        {
            if(inDungeon && hasDungeon)
            {
                if (dtextureGenerated)
                {
                    return backgroundTexture;
                }
                else
                {
                    if (dbackgroundBitmap == null || textureOld)
                    {
                        dbackgroundBitmap = Managers.TextureManager.BitmapFrom2DTileMap(backgroundTiles);
                        backgroundTexture = Managers.TextureManager.TextureFromBitmap(dbackgroundBitmap);
                        dtextureGenerated = true;
                        textureGenerated = false;
                        return backgroundTexture;
                    }
                    else
                    {
                        backgroundTexture = Managers.TextureManager.TextureFromBitmap(dbackgroundBitmap);
                        dtextureGenerated = true;
                        textureGenerated = false;
                        return backgroundTexture;
                    }
                }
            }
            else
            {
                if (textureGenerated)
                {
                    return backgroundTexture;
                }
                else
                {
                    if (backgroundBitmap == null || textureOld)
                    {
                        backgroundBitmap = Managers.TextureManager.BitmapFrom2DTileMap(backgroundTiles);
                        backgroundTexture = Managers.TextureManager.TextureFromBitmap(backgroundBitmap);
                        textureGenerated = true;
                        return backgroundTexture;
                    }
                    else
                    {
                        backgroundTexture = Managers.TextureManager.TextureFromBitmap(backgroundBitmap);
                        textureGenerated = true;
                        return backgroundTexture;
                    }
                }
            }
        }

        public void AfterDiskLoad(TextureAtlas backgroundtAtlas)
        {
            WorldManager.man.wGen.wData = wData;

            if(hasDungeon)
            {
                dungeon.AfterDiskLoad(backgroundtAtlas);
            }

            for(int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    backgroundTiles[x, y].graphics.tAtlas = backgroundtAtlas;
                    foregroundTiles[x, y].graphics.tAtlas = backgroundtAtlas;
                }
            }
        }
    }
}
