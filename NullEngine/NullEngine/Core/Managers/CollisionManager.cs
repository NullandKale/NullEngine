using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;

namespace nullEngine.Managers
{
    /*========================================================================================================
     *  
     * The following contains an optimization that requires a bit of explaination
     * 
     * Instead of keeping a list of all the objects that are collidable in one large list and then 
     * checking all the objects everytime anything wants to know if there is a collision this keeps 
     * a dictionary where the key is a bounding box in space and the value is a list of all the objects 
     * in that bounding box that way whenever anything wants to know if there is a collision it can
     * just check the objects around it.
     * 
     *=======================================================================================================*/
    public class CollisionManager
    {
        //static singleton reference
        public static CollisionManager man;

        //bounding box dict
        public Dictionary<Point, List<Entity___Component.cCollider>> boundingBoxes;

        //bounding box size
        int boundSize;

        public CollisionManager(int minBoundSize)
        {
            //singleton management
            if(man == null)
            {
                man = this;
            }
            else
            {
                throw new SingletonException(this);
            }

            //set the minimun bound size
            boundSize = minBoundSize;

            // initialize the bounding box dict
            boundingBoxes = new Dictionary<Point, List<Entity___Component.cCollider>>(new pointHashCode());
        }

        //add the collider to the list in the dict
        public static void addCollider(Entity___Component.cCollider c)
        {
            //generate the key
            Point key = getKey(c.rect);
            //if there is already a list for that key just put the collider in that list ~~
            if(man.boundingBoxes.ContainsKey(key))
            {
                man.boundingBoxes[key].Add(c);
            }
            else
            {
                //~~ otherwise add a new list to that key and add the collider 
                man.boundingBoxes.Add(key, new List<Entity___Component.cCollider>());
                man.boundingBoxes[key].Add(c);
            }
            //set the colliders key to the generated key
            c.key = key;
        }

        //remove the collider from the bounding box dict
        public static void removeCollider(Entity___Component.cCollider c)
        {
            man.boundingBoxes[c.key].Remove(c);
            if(man.boundingBoxes[c.key].Count <= 0)
            {
                man.boundingBoxes.Remove(c.key);
            }
        }

        //remove and replace the collider to change which bounding box it is in
        public static void moveCollider(Entity___Component.cCollider c)
        {
            removeCollider(c);
            addCollider(c);
        }

        //generate a key based off of the position of a rect
        public static Point getKey(Rectangle rect)
        {
            return new Point(rect.X / man.boundSize, rect.Y / man.boundSize);
        }


        public static Point WillItCollide(Entity___Component.cCollider c, int xMove, int yMove)
        {
            //assign the movement to a point to use to return
            Point p = new Point(xMove, yMove);

            //Create two rects that corrispond to the rect if this move is allowed for each axis
            Rectangle futureRectX = new Rectangle(c.rect.X + xMove, c.rect.Y, c.rect.Width, c.rect.Height);
            Rectangle futureRectY = new Rectangle(c.rect.X, c.rect.Y + yMove, c.rect.Width, c.rect.Height);

            //check if each of those rects will collide
            bool collideX = man.CheckFutureCollision(futureRectX, c);
            bool collideY = man.CheckFutureCollision(futureRectY, c);

            //if the rect that corrisponds to the xmove collides set the xmove to 0
            if(collideX)
            {
                if(xMove < 0)
                {
                    p.X = 1;
                }
                else
                {
                    p.X = -1;
                }
            }

            //do the same for the y move
            if(collideY)
            {
                if (yMove < 0)
                {
                    p.Y = 1;
                }
                else
                {
                    p.Y = -1;
                }
            }

            //return the adjusted movement
            return p;
        }

        public List<Entity___Component.cCollider> CheckCollision(Entity___Component.cCollider c)
        {
            //create a list to hold all of the objects that might be colliding
            List<Entity___Component.cCollider> temp = new List<Entity___Component.cCollider>();

            //get the key for the collidable that is being checked 
            Point cKey = getKey(c.rect);

            //for all of the surrounding bounding boxes --
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    //-- generate the key for that bounding box --
                    Point key = new Point(cKey.X + i, cKey.Y + j);
                    
                    //-- if the key has any objects in it -- 
                    if (boundingBoxes.ContainsKey(key))
                    {
                        //-- for all of said objects ~~
                        List<Entity___Component.cCollider> box = boundingBoxes[key];
                        for (int k = 0; k < box.Count; k++)
                        {
                            //~~ if the object collides with the calling object and it is not the calling object ~~
                            if (c.collides(box[k]) && c != box[k])
                            {
                                //~~ add that object to the list to return
                                temp.Add(box[k]);
                            }
                        }
                    }
                }
            }

            //return all the objects that are colliding
            return temp;
        }

        public List<Entity___Component.cCollider> CheckCollision(Entity___Component.cCollider c, int buffer)
        {
            //create a list to hold all of the objects that might be colliding
            List<Entity___Component.cCollider> temp = new List<Entity___Component.cCollider>();

            //get the key for the collidable that is being checked 
            Point cKey = getKey(c.rect);
            Rectangle rect = new Rectangle(c.rect.X - buffer, c.rect.Y - buffer, c.rect.Width + (buffer * 2), c.rect.Height + (buffer * 2));

            //for all of the surrounding bounding boxes --
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    //-- generate the key for that bounding box --
                    Point key = new Point(cKey.X + i, cKey.Y + j);

                    //-- if the key has any objects in it -- 
                    if (boundingBoxes.ContainsKey(key))
                    {
                        //-- for all of said objects ~~
                        List<Entity___Component.cCollider> box = boundingBoxes[key];
                        for (int k = 0; k < box.Count; k++)
                        {
                            //~~ if the object collides with the calling object and it is not the calling object ~~
                            if (box[k].collides(rect, c) && c != box[k])
                            {
                                //~~ add that object to the list to return
                                temp.Add(box[k]);
                            }
                        }
                    }
                }
            }

            //return all the objects that are colliding
            return temp;
        }

        public Boolean CheckForLeaveWorld(Rectangle rect)
        {
            //if the given rect is outside the world even a little bit return true
            if(rect.X + rect.Width > Game.worldRect.Right)
            {
                return true;
            }

            if(rect.X < Game.worldRect.Left)
            {
                return true;
            }

            if(rect.Y + rect.Height > Game.worldRect.Bottom)
            {
                return true;
            }

            if(rect.Y < Game.worldRect.Top)
            {
                return true;
            }

            //if all else fails return false
            return false;

        }

        //this checks if a rect will cause a collision and returns true if it does
        public Boolean CheckFutureCollision(Rectangle rect, Entity___Component.cCollider c)
        {
            //get the boundingBoxes Key from the rect
            Point cKey = getKey(rect);

            //if the rect is outside the world immediately return true 
            if (c.rRef.tag != "Player" && CheckForLeaveWorld(rect))
            {
                return true;
            }

                //for all of the surrounding bounding boxes check if any of the objects will collide
                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        //this generates the key for the surrounding bounding boxes
                        Point key = new Point(cKey.X + i, cKey.Y + j);

                        //this checks if the corrisponding bounding box has any objects in it
                        if (boundingBoxes.ContainsKey(key))
                        {
                            List<Entity___Component.cCollider> box = boundingBoxes[key];
                            //for every object in the bounding box --
                            for (int k = 0; k < box.Count; k++)
                            {
                                //-- check if the bounding box collides and is not the object calling this function return true
                                if (box[k].collides(rect, c) && c != box[k])
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
            //if all else fails return false
            return false;
        }
    }
}

//this is a replacement for System.Drawing.Point's hashcode because it was just obj.x xor obj.y and that caused a TON of collisions
public class pointHashCode : IEqualityComparer<Point>
{
    public bool Equals(Point x, Point y)
    {
        if(x.X == y.X && x.Y == y.Y)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //set bitshift x 16 to the right and then clear the top 16 bits of Y and add the two together
    public int GetHashCode(Point obj)
    {
        return obj.X << 16 + (short)obj.Y;
    }
}
