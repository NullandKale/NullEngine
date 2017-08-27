using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System.Drawing;
using nullEngine.Component;

namespace nullEngine.Entity
{
    public class quad : renderable
    {
        public int width;
        public int height;
        TextureAtlas atlas;
        int texID;

        //Construct with single texture file
        public quad(string textureLocation)
        {
            pos = new transform();
            components = new List<iComponent>();
            tex = Managers.TextureManager.LoadTexture(textureLocation, false);
            height = tex.height;
            width = tex.width;
        }

        public quad(Texture2D texture)
        {
            pos = new transform();
            components = new List<iComponent>();
            tex = texture;
            height = tex.height;
            width = tex.width;
        }

        //Construct with texture atlas and Texture ID
        public quad(TextureAtlas tAtlas, int id)
        {
            pos = new transform();
            components = new List<iComponent>();
            atlas = tAtlas;
            texID = id;
            tex = tAtlas.getTile(id);
            height = tAtlas.tilePixelHeight;
            width = tAtlas.tilePixelWidth;
        }

        public override void update()
        {
            base.DistCulling();

            //Loop through all componants and run them.
            for (int i = 0; i < components.Count; i++)
            {
                components[i].Run(this);
            }

            //At end of update add renderer to render Queue.
            if (active && culled)
            {
                Game.renderQueue.Enqueue(render);
            }
        }

        public override void render()
        {
            pos.updateMatrix();
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref pos.modelViewMatrix);

            //Replace GL command with cached texture set.
            //This function only sets the texture if it isnt already set.
            //GL.BindTexture(TextureTarget.Texture2D, tex.id);
            Managers.TextureManager.GLSetTexture(tex.id);

            GL.Begin(PrimitiveType.Triangles);

            GL.TexCoord2(tex.xStart, tex.yStart);
            GL.Vertex2(0, 0);

            GL.TexCoord2(tex.xEnd, tex.yEnd);
            GL.Vertex2(width, height);

            GL.TexCoord2(tex.xStart, tex.yEnd);
            GL.Vertex2(0, height);

            GL.TexCoord2(tex.xStart, tex.yStart);
            GL.Vertex2(0, 0);

            GL.TexCoord2(tex.xEnd, tex.yStart);
            GL.Vertex2(width, 0);

            GL.TexCoord2(tex.xEnd, tex.yEnd);
            GL.Vertex2(width, height);

            GL.End();
        }

        public override int getWidth()
        {
            return tex.width * transform.masterScale;
        }

        public override int getHeight()
        {
            return tex.height * transform.masterScale;
        }
    }
}
