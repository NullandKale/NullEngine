namespace nullEngine
{
    public class TextureAtlas
    {
        //total atlas pixel size
        public int pixelWidth;
        public int pixelHeight;

        //tile pixel size
        public int tilePixelWidth;
        public int tilePixelHeight;
        private int _padding;

        //texture atlas tile sizes
        public int tileWidth;
        public int tileHeight;

        //texture info
        public string path;
        private Texture2D baseTexture;
        public bool notEmpty;

        public TextureAtlas(string TexturePath, int xTileCount, int yTileCount, int pixelsPerTileX, int pixelsPerTileY, int padding)
        {
            //get base texture
            baseTexture = Managers.TextureManager.LoadTexture(TexturePath, false);

            //set variables based off of constructor
            path = TexturePath;

            pixelWidth = baseTexture.width + 1;
            pixelHeight = baseTexture.height + 1;

            tilePixelHeight = pixelsPerTileY;
            tilePixelWidth = pixelsPerTileX;

            tileWidth = xTileCount;
            tileHeight = yTileCount;
            _padding = padding;

            notEmpty = true;
        }

        //get a textuee for the tile
        public Texture2D getTile(int index)
        {
            //determine the x and y pixel offset
            int xOffset = (index % tileWidth);
            int yOffset = (index / tileWidth);

            float xPixels, yPixels, xPixEnd, yPixEnd;

            //calculate xstart and xend locations
            if(xOffset != 0)
            {
                xPixels = ((float)xOffset * (tilePixelWidth + _padding)) / pixelWidth;
                xPixEnd = (((float)xOffset * (tilePixelWidth + _padding)) + tilePixelWidth) / pixelWidth;
            }
            else
            {
                xPixels = 0f;
                xPixEnd = (float)tilePixelWidth / pixelWidth;
            }

            //calculate ystart and yend pixel locations
            if (yOffset != 0)
            {
                yPixels = ((float)yOffset * (tilePixelHeight + _padding)) / pixelHeight;
                yPixEnd = (((float)yOffset * (tilePixelHeight + _padding)) + tilePixelHeight) / pixelHeight;
            }
            else
            {
                yPixels = 0f;
                yPixEnd = (float)tilePixelHeight / pixelHeight;
            }

            //generate texture and return
            return new Texture2D(baseTexture.id, pixelWidth, pixelHeight, xPixels, yPixels, xPixEnd, yPixEnd);
        }
    }
}
