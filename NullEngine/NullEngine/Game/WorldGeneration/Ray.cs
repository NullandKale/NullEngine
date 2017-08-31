using System;
using System.Collections.Generic;
using System.Drawing;

namespace NullGame.WorldGen
{
    [Serializable]
    public class Ray
    {
        public Point start;
        public Point end;
        public long a;
        public long b;
        public long c;
        public double Dist;

        private bool isHorz;
        private bool isVert;

        public Ray(Point rayStart, Point rayEnd)
        {
            start = rayStart;
            end = rayEnd;

            isVert = start.X == end.X;
            isHorz = start.Y == end.Y;

            a = end.Y - start.Y;
            b = start.X - end.X;
            c = a * start.X + a * start.Y;

            double XSquared = Math.Pow(Math.Abs(start.X - end.X), 2);

            double YSquared = Math.Pow(Math.Abs(start.Y - end.Y), 2);

            Dist = Math.Sqrt(XSquared + YSquared);
        }

        public static bool isIntersectingPoint(Ray r, Point p)
        {
            Ray StartToMid = new Ray(r.start, p);
            Ray MidToEnd = new Ray(p, r.end);

            double variance = 0.02d; //double.Epsilon * 100000d;

            if (StartToMid.Dist + MidToEnd.Dist >= r.Dist - variance && StartToMid.Dist + MidToEnd.Dist <= r.Dist + variance)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static List<Point> isIntersecting(Ray r1, Ray r2)
        {
            if(!(r1.isVert || r1.isHorz || r2.isHorz || r2.isVert))
            {
                throw new NotImplementedException("ray interection is only supported if one ray is vertical or horizontal");
            }

            List<Point> temp = new List<Point>();

            if(r1.isHorz)
            {
                for(int i = 0; i < r1.Dist; i++)
                {
                    Point currentPoint = new Point(r1.start.X, r1.start.Y + i);
                    if(Ray.isIntersectingPoint(r2, currentPoint))
                    {
                        temp.Add(currentPoint);
                    }
                }
            }
            
            if(r1.isVert)
            {
                for (int i = 0; i < r1.Dist; i++)
                {
                    Point currentPoint = new Point(r1.start.X + i, r1.start.Y);
                    if (Ray.isIntersectingPoint(r2, currentPoint))
                    {
                        temp.Add(currentPoint);
                    }
                }
            }

            if(r2.isHorz)
            {
                for (int i = 0; i < r2.Dist; i++)
                {
                    Point currentPoint = new Point(r2.start.X, r2.start.Y + i);
                    if (Ray.isIntersectingPoint(r1, currentPoint))
                    {
                        temp.Add(currentPoint);
                    }
                }
            }

            if(r2.isVert)
            {
                for (int i = 0; i < r2.Dist; i++)
                {
                    Point currentPoint = new Point(r2.start.X + i, r2.start.Y);
                    if (Ray.isIntersectingPoint(r1, currentPoint))
                    {
                        temp.Add(currentPoint);
                    }
                }
            }

            return temp;
        }

        public static List<Point> pointsIntersectingRect(Ray r, Rectangle rect)
        {
            List<Point> temp = new List<Point>();
            for(int x = 0; x < rect.Width; x++)
            {
                for (int y = 0; y < rect.Height; y++)
                {
                    Point currentPoint = new Point(rect.X + x, rect.Y + y);
                    if (Ray.isIntersectingPoint(r, currentPoint))
                    {
                        //Console.WriteLine("[" + currentPoint.X + "," + currentPoint.Y + "]");
                        temp.Add(currentPoint);
                    }
                }
            }
            return temp;
        }

        public static bool isIntersectingRect(Ray r, Rectangle rect)
        {
            for (int x = 0; x < rect.Width; x++)
            {
                for (int y = 0; y < rect.Height; y++)
                {
                    Point currentPoint = new Point(rect.X + x, rect.Y + y);
                    if (Ray.isIntersectingPoint(r, currentPoint))
                    {
                        //Console.WriteLine("[" + currentPoint.X + "," + currentPoint.Y + "]");
                        return true;
                    }
                }
            }

            return false;
        }
    }
}