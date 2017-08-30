using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;
using NullEngine.Component;
using NullEngine.WorldGen;

namespace NullEngine.Entity
{
    //this is an entity whose texture is text
    public class text : renderable
    {
        //store the tiles for this text
        public Tile[] tiles;

        //store the string in this Text
        public string textString;

        //construct a text entity based off of a letter array
        public text(letter[] letters)
        {
            tiles = new Tile[letters.Length];
            pos = new transform();
            components = new List<iComponent>();

            //create the tile array
            for(int i = 0; i < letters.Length; i++)
            {
                tiles[i] = new Tile();
                tiles[i].TexID = (int)letters[i];
                tiles[i].tAtlas = Game.font;
            }

            //generate the texture based off of the tile array
            tex = Managers.TextureManager.TextureFrom1DTileMap(tiles);
            textString = "";
        }

        //construct a text entity based off of a string
        public text(string s)
        {
            letter[] letters = stringToLetter(s);
            tiles = new Tile[letters.Length];
            pos = new transform();
            components = new List<iComponent>();

            //create the tile array
            for (int i = 0; i < letters.Length; i++)
            {
                tiles[i] = new Tile();
                tiles[i].TexID = (int)letters[i];
                tiles[i].tAtlas = Game.font;
            }

            //generate the texture based off of the tile array
            tex = Managers.TextureManager.TextureFrom1DTileMap(tiles);
            textString = s;
        }

        public override void update()
        {
            //if the object is active and on screen
            if(active && culled)
            {
                //enqueue the render function
                Game.renderQueue.Enqueue(render);

                for(int i = 0; i < components.Count; i++)
                {
                    components[i].Run(this);
                }
            }

            //do the distance culling 
            base.DistCulling();
        }

        public void ChangeText(string s)
        {
            if(s != textString)
            {
               letter[] letters = stringToLetter(s);
                tiles = new Tile[letters.Length];
                for (int i = 0; i < letters.Length; i++)
                {
                    tiles[i] = new Tile();
                    tiles[i].TexID = (int)letters[i];
                    tiles[i].tAtlas = Game.font;
                }
                Managers.TextureManager.GLDestoryTexture(tex.id);
                tex = Managers.TextureManager.TextureFrom1DTileMap(tiles);
            }
        }

        //this is the basic render command
        public override void render()
        {
            //update the transformation matrix
            pos.updateMatrix();
            //load the matrix into the GPU
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref pos.modelViewMatrix);

            //Replace GL command with cached texture set.
            //This function only sets the texture if it isnt already set.
            //GL.BindTexture(TextureTarget.Texture2D, tex.id);
            Managers.TextureManager.GLSetTexture(tex.id);

            //this draws the rect, setting the texture first and then placing the vertex
            GL.Begin(PrimitiveType.Triangles);

            GL.TexCoord2(tex.xStart, tex.yStart);
            GL.Vertex2(0, 0);

            GL.TexCoord2(tex.xEnd, tex.yEnd);
            GL.Vertex2(tex.width, tex.height);

            GL.TexCoord2(tex.xStart, tex.yEnd);
            GL.Vertex2(0, tex.height);

            GL.TexCoord2(tex.xStart, tex.yStart);
            GL.Vertex2(0, 0);

            GL.TexCoord2(tex.xEnd, tex.yStart);
            GL.Vertex2(tex.width, 0);

            GL.TexCoord2(tex.xEnd, tex.yEnd);
            GL.Vertex2(tex.width, tex.height);

            //this finishes the current object draw
            GL.End();
        }

        //performs a transform for the width into actual pixels on the screen
        public override int getWidth()
        {
            return tex.width * transform.masterScale;
        }

        //performs a transform for the height into actual pixels on the screen
        public override int getHeight()
        {
            return tex.height * transform.masterScale;
        }

        //this just runs the charToLetter command on every char in a string and returns a letter array
        public static letter[] stringToLetter(string s)
        {
            char[] c = s.ToCharArray();
            letter[] l = new letter[c.Length];

            for(int i = 0; i < c.Length; i++)
            {
                l[i] = charToLetter(c[i]);
            }

            return l;
        }

        //this converts a char to a letter as the chars are ordered in the ansi order
        public static letter charToLetter(char c)
        {
            return (letter)((int)c - (int)' ');
        }
    }
}
