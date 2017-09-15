using System;
using System.Collections.Generic;
using OpenTK;
using System.Drawing;
using NullEngine.Component;

namespace NullEngine.Entity
{
    public abstract class renderable
    {
        public transform pos;
        public List<triangle> verts;
        protected List<iComponent> components;
        public Texture2D tex;
        public Color col;
        public bool active = true;
        public StateMachine.iState parentState;
        public bool culled = false;
        public bool doDistCulling = true;
        public string tag;

        public abstract int getWidth();
        public abstract int getHeight();
        public abstract void update();
        public abstract void render();

        public Rectangle getRect()
        {
            return new Rectangle((int)pos.xPos, (int)pos.yPos, (int)(getWidth() * pos.xScale), (int)(getHeight() * pos.yScale));
        }

        public void OnDestroy()
        {
            foreach (iComponent c in components)
            {
                c.OnDestroy(this);
            }
        }

        public void DistCulling()
        {
            if (doDistCulling)
            {
                if (active)
                {
                    if (Game.windowRect.IntersectsWith(getRect()))
                    {
                        culled = true;
                    }
                    else
                    {
                        culled = false;
                    }
                }
            }
            else
            {
                culled = true;
            }
        }

        public void AddComponent(iComponent c)
        {
            //prevent adding multiple of the same component
            if(!components.Contains(c))
            {
                components.Add(c);
            }
        }

        public int FindComponent<T> ()
        {
            for(int i = 0; i < components.Count; i++)
            {
                Type compType = components[i].GetType();
                if (compType is T)
                {
                    return i;
                }
            }
            return -1;
        }

        //find the component of type T
        public T GetComponent<T> ()
        {
            //get the compenent 
            int temp = FindComponent<T>();
            if (temp == -1)
            {
                throw new Exception("Cannot Find Component");
            }
            return (T)components[FindComponent<T>()];
        }

        public void setPos(int x, int y)
        {
            pos.xPos = x;
            pos.yPos = y;
        }

        public void setPos(Point p)
        {
            pos.xPos = p.X;
            pos.yPos = p.Y;
        }

        public void setRelativePos(Point p)
        {
            pos.xPos += p.X;
            pos.yPos += p.Y;
        }

        public void setRelativePos(float x, float y)
        {
            pos.xPos += x;
            pos.yPos += y;
        }
    }

    public class transform
    {
        //data storage
        public static int masterScale = 4;
        public float xPos = 0;
        public float yPos = 0;
        public float zPos = 0;

        public float rotZ = 0;

        public float xScale = 1;
        public float yScale = 1;

        public Matrix4 modelViewMatrix;

        //update the modelViewMatrix based off of the stored info
        //if it wasnt for this function this would be a struct
        public void updateMatrix()
        {
              modelViewMatrix = Matrix4.CreateScale(xScale * masterScale, yScale * masterScale, 1f) * 
                Matrix4.CreateRotationZ(rotZ) * 
                Matrix4.CreateTranslation(xPos, yPos, zPos);
        }
    }

    //this struct defines the location for all of the verticies for a triangle
    public struct triangle
    {
        public Vector2 a;
        public Vector2 b;
        public Vector2 c;
    }
}
