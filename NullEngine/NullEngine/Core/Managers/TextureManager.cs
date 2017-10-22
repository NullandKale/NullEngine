using System;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System.Drawing.Imaging;
using NullEngine.Entity;

namespace NullEngine.Managers
{
    //this class handles loading and setting textures in GPU memory
    public class TextureManager
    {
        //this is the texture ID for the texture currently used by OpenGL
        static int currentTexture = -1;

        //this is the texture load function
        public static Texture2D LoadTexture(string filePath, bool LinearFiltering)
        {
            //get the bitmap from the image on disk
            Bitmap bitmap = new Bitmap(filePath);

            //get the next available texture ID
            int id = GL.GenTexture();

            //lock the texture data
            BitmapData bmpData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            //bind the texture ID to the location in GPU memory 
            GL.BindTexture(TextureTarget.Texture2D, id);

            //set the texture to that memmory
            GL.TexImage2D(TextureTarget.Texture2D, 0,
                PixelInternalFormat.Rgba, bitmap.Width, bitmap.Height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra,
                PixelType.UnsignedByte, bmpData.Scan0);

            //unlock the texture data
            bitmap.UnlockBits(bmpData);

            //if linear filtering is desired set the texture options as such
            if(LinearFiltering)
            {
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter,
                    (int)TextureMinFilter.Linear);

                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter,
                    (int)TextureMagFilter.Linear);
            } // if not set them with nearest as the filtering option
            else
            {
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter,
                    (int)TextureMinFilter.Nearest);

                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter,
                    (int)TextureMinFilter.Nearest);
            }
            //return all the texture data in a texture2D object
            return new Texture2D(id, bitmap.Width, bitmap.Height);
        }

        

        public static Texture2D TextureFromBitmap(Bitmap final)
        {
            int id = GL.GenTexture();

            Debug.Text("Locking bitmap and sending to graphics memory");
            BitmapData bmpData = final.LockBits(new Rectangle(0, 0, final.Width, final.Height),
                    ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.BindTexture(TextureTarget.Texture2D, id);

            GL.TexImage2D(TextureTarget.Texture2D, 0,
                PixelInternalFormat.Rgba, final.Width, final.Height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra,
                PixelType.UnsignedByte, bmpData.Scan0);

            final.UnlockBits(bmpData);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter,
                (int)TextureMinFilter.Nearest);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter,
                (int)TextureMagFilter.Nearest);

            return new Texture2D(id, final.Width, final.Height);
        }
        
        //this checks what the current working texture is and if the desired texture is different sets that as the working texture
        public static void GLSetTexture(int id)
        {
            if(currentTexture == -1 || currentTexture != id)
            {
                GL.BindTexture(TextureTarget.Texture2D, id);
                currentTexture = id;
            }
        }

        //this function removes a texture from memory
        //ONLY USE IF SURE THE TEXTURE IS NOT IN USE SOMEWHERE ELSE
        public static void GLDestoryTexture(int id)
        {
            GL.DeleteTexture(id);
        }

        //take a 1 dimentional tilemap and create one texture for the whole thing
        public static Texture2D TextureFrom1DTileMap(Tile[] tiles)
        {
            //get tile info
            int tileSizeX = tiles[0].tAtlas.tilePixelWidth;
            int tileSizeY = tiles[0].tAtlas.tilePixelHeight;
            int xTileCount = tiles[0].tAtlas.tileWidth;
            int yTileCount = tiles[0].tAtlas.tileHeight;
            String filePath = tiles[0].tAtlas.path;

            //get the image data from the disk
            Bitmap atlas = new Bitmap(filePath);

            //create a new bitmap with the correct dimensions for all of the tiles
            Bitmap final = new Bitmap(tileSizeX * tiles.Length + 1, tileSizeY);
            int id = GL.GenTexture();

            //this loop selects the current tile that is being put on the new bitmap
            for (int i = 0; i < tiles.Length; i++)
            {
                //these generate the pixel positions of the tile
                int tilePosY = tiles[i].TexID / xTileCount;
                int tilePosX = tiles[i].TexID % xTileCount;

                //these loops copy the pixels from the tiles texture into the new texture
                for (int k = 0; k < tileSizeY; k++)
                {
                    for (int j = 0; j < tileSizeX; j++)
                    {
                        int xCord = (i * tileSizeX) + j;
                        int yCord = k;

                        int tileCordX = ((tilePosX) * tileSizeX) + j;
                        int tileCordY = ((tilePosY) * tileSizeY) + k;

                        Color c = atlas.GetPixel(tileCordX, tileCordY);

                        final.SetPixel(xCord, yCord, c);
                    }
                }
            }
            //this locks the new texture data
            BitmapData bmpData = final.LockBits(new Rectangle(0, 0, final.Width, final.Height),
                    ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            //bind the texture ID to the location in GPU memory 
            GL.BindTexture(TextureTarget.Texture2D, id);

            //set the texture to that memmory
            GL.TexImage2D(TextureTarget.Texture2D, 0,
                PixelInternalFormat.Rgba, final.Width, final.Height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra,
                PixelType.UnsignedByte, bmpData.Scan0);

            //unlock the new texture data
            final.UnlockBits(bmpData);

            //set the new textures options
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter,
                (int)TextureMinFilter.Nearest);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter,
                (int)TextureMagFilter.Nearest);

            //return the new texture
            return new Texture2D(id, final.Width, final.Height);

        }

        //this operates the same as above but in 2 dimmensions not 1
        public static Texture2D TextureFrom2DTileMap(Tile[,] tiles)
        {
            int tileSizeX = tiles[0, 0].tAtlas.tilePixelWidth;
            int tileSizeY = tiles[0, 0].tAtlas.tilePixelHeight;
            int xTileCount = tiles[0, 0].tAtlas.tileWidth;
            int yTileCount = tiles[0, 0].tAtlas.tileHeight;
            String filePath = tiles[0, 0].tAtlas.path;

            Bitmap atlas = new Bitmap(filePath);

            Bitmap final = new Bitmap(tileSizeX * tiles.GetLength(0) + 1, tileSizeY * tiles.GetLength(1) + 1);
            int id = GL.GenTexture();

            for (int x = 0; x < tiles.GetLength(0); x++)
            {
                for (int y = 0; y < tiles.GetLength(1); y++)
                {
                    int tilePosY = tiles[x, y].TexID / xTileCount;
                    int tilePosX = tiles[x, y].TexID % xTileCount;

                    for (int k = 0; k < tileSizeY; k++)
                    {
                        for (int j = 0; j < tileSizeX; j++)
                        {
                            int xCord = (x * tileSizeX) + j;
                            int yCord = (y * tileSizeY) + k;

                            int tileCordX = ((tilePosX) * tileSizeX) + j;
                            int tileCordY = ((tilePosY) * tileSizeY) + k;

                            Color c = atlas.GetPixel(tileCordX, tileCordY);

                            final.SetPixel(xCord, yCord, c);
                        }
                    }
                }
            }

            BitmapData bmpData = final.LockBits(new Rectangle(0, 0, final.Width, final.Height),
                    ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.BindTexture(TextureTarget.Texture2D, id);

            GL.TexImage2D(TextureTarget.Texture2D, 0,
                PixelInternalFormat.Rgba, final.Width, final.Height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra,
                PixelType.UnsignedByte, bmpData.Scan0);

            final.UnlockBits(bmpData);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter,
                (int)TextureMinFilter.Nearest);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter,
                (int)TextureMagFilter.Nearest);

            return new Texture2D(id, final.Width, final.Height);

        }

        // PLEASE FIX THIS!
        public static Bitmap BitmapFrom2DTileMap(worldTile[,] tiles)
        {
            Debug.Text("Generating 2D tileMap bitmap");
            int tileSizeX = tiles[0, 0].graphics.tAtlas.tilePixelWidth;
            int tileSizeY = tiles[0, 0].graphics.tAtlas.tilePixelHeight;
            int xTileCount = tiles[0, 0].graphics.tAtlas.tileWidth;
            int yTileCount = tiles[0, 0].graphics.tAtlas.tileHeight;
            String filePath = tiles[0, 0].graphics.tAtlas.path;

            int tilesLengthX = tiles.GetLength(0);
            int tilesLengthY = tiles.GetLength(1);

            Debug.Text("Loading Tile Atlas");
            Bitmap atlas = new Bitmap(filePath);

            Debug.Text("Generating final bitmap");
            Bitmap final = new Bitmap(tileSizeX * tilesLengthX, tileSizeY * tilesLengthY);

            Rectangle rect = new Rectangle(0, 0, final.Width, final.Height);
            BitmapData bmpData = final.LockBits(rect, ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            Debug.Text("Setting final bitmap pixels");
            System.Diagnostics.Stopwatch s = new System.Diagnostics.Stopwatch();
            s.Start();
            
            int bmpByteWidth = Math.Abs(bmpData.Stride);
            int bytesCount = bmpByteWidth * final.Height;

            byte[] colorBytes = new byte[bytesCount];

            int currentByte = 0;

            Debug.Text(final.Height + ", " + final.Width + ", " + bmpByteWidth + ", " + colorBytes.Length);

            var lastX = -1;
            var render = false;
            var count = 0;
            var coords = string.Empty;

            for (int y = 0; y < final.Height - 1; y++)
            {
                count = 0;
                render = true;
                coords = "{ ";
                for (int x = 0; x < bmpByteWidth - 4; x++)
                {
                    int xTile = (x / 4) / tileSizeX;
                    int yTile = y / tileSizeY;

                    int tilePosY = tiles[xTile, yTile].graphics.TexID / xTileCount;
                    int tilePosX = tiles[xTile, yTile].graphics.TexID % xTileCount;

                    int XPixelPos = tilePosX * tileSizeX + ((x / 4) % tileSizeX);
                    int YPixelPos = tilePosY * tileSizeY + (y % tileSizeY);

                    if (XPixelPos != lastX && render == true)
                    {
                        coords += "(" + XPixelPos + ", " + YPixelPos + "), ";
                        lastX = XPixelPos;
                        if (count >= 16)
                        {
                            count = 0;
                            render = false;
                        }
                        else
                        {
                            count++;
                        }
                    }

                    switch (currentByte)
                    {
                        case 0:
                            colorBytes[y * final.Height + x] = atlas.GetPixel(XPixelPos, YPixelPos).B;
                            currentByte++;
                            break;

                        case 1:
                            colorBytes[y * final.Height + x] = atlas.GetPixel(XPixelPos, YPixelPos).G;
                            currentByte++;
                            break;

                        case 2:
                            colorBytes[y * final.Height + x] = atlas.GetPixel(XPixelPos, YPixelPos).R;
                            currentByte++;
                            break;

                        case 3:
                            colorBytes[y * final.Height + x] = atlas.GetPixel(XPixelPos, YPixelPos).A;
                            currentByte = 0;
                            break;
                    }
                }
                coords += "}";
                Debug.Warning("Y: " + y);
                Debug.Text(coords);
            }

            IntPtr ptr = bmpData.Scan0;
            System.Runtime.InteropServices.Marshal.Copy(colorBytes, 0, ptr, bytesCount);
            final.UnlockBits(bmpData);
            s.Stop();
            
            Debug.Text("This took: " + s.ElapsedMilliseconds + " ms");
            // Debug.Text(final.Height + ", " + final.Width);

            return final;
        }


        public static Texture2D TextureFrom2DTileMap(worldTile[,] tiles)
        {
            Debug.Text("Generating 2D tileMap Texture");
            int tileSizeX = tiles[0, 0].graphics.tAtlas.tilePixelWidth;
            int tileSizeY = tiles[0, 0].graphics.tAtlas.tilePixelHeight;
            int xTileCount = tiles[0, 0].graphics.tAtlas.tileWidth;
            int yTileCount = tiles[0, 0].graphics.tAtlas.tileHeight;
            String filePath = tiles[0, 0].graphics.tAtlas.path;

            Debug.Text("Loading Tile Atlas");
            Bitmap atlas = new Bitmap(filePath);

            Debug.Text("Generating final bitmap");
            Bitmap final = new Bitmap(tileSizeX * tiles.GetLength(0) + 1, tileSizeY * tiles.GetLength(1) + 1);
            int id = GL.GenTexture();

            Debug.Text("Setting final bitmap pixels");
            for (int x = 0; x < tiles.GetLength(0); x++)
            {
                for (int y = 0; y < tiles.GetLength(1); y++)
                {
                    int tilePosY = tiles[x, y].graphics.TexID / xTileCount;
                    int tilePosX = tiles[x, y].graphics.TexID % xTileCount;

                    for (int k = 0; k < tileSizeY; k++)
                    {
                        for (int j = 0; j < tileSizeX; j++)
                        {
                            int xCord = (x * tileSizeX) + j;
                            int yCord = (y * tileSizeY) + k;

                            int tileCordX = ((tilePosX) * tileSizeX) + j;
                            int tileCordY = ((tilePosY) * tileSizeY) + k;

                            Color c = atlas.GetPixel(tileCordX, tileCordY);

                            final.SetPixel(xCord, yCord, c);
                        }
                    }
                }
            }

            Debug.Text("Locking bitmap and sending to graphics memory");
            BitmapData bmpData = final.LockBits(new Rectangle(0, 0, final.Width, final.Height),
                    ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.BindTexture(TextureTarget.Texture2D, id);

            GL.TexImage2D(TextureTarget.Texture2D, 0,
                PixelInternalFormat.Rgba, final.Width, final.Height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra,
                PixelType.UnsignedByte, bmpData.Scan0);

            final.UnlockBits(bmpData);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter,
                (int)TextureMinFilter.Nearest);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter,
                (int)TextureMagFilter.Nearest);

            return new Texture2D(id, final.Width, final.Height);

        }
    }
}
