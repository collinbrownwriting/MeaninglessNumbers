using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Meaningless_Numbers
{
    public class Field
    {
        public Point[,] points;
        public int threshold;
        public float tension;
        public bool interacts;
        public bool hasHighParticles;

        public void InterFieldInteractions(Point a, Field f)
        {
            Point b = points[a.x, a.y];
            float diffuse = (a.vector.mag + b.vector.mag / f.threshold) * (1 / f.tension);
            if (a.vector.mag - diffuse > 0)
            {
                b.vector.mag += diffuse;
                a.vector.mag -= diffuse;
            }
        }

        public void HandleRipples(List<Field> fields, int x, int y)
        {
            Point activePoint = this.points[x, y];

            if (interacts && activePoint.vector.mag != 0)
            {
                foreach (Field f in fields)
                {
                    if (f != this && activePoint.vector.mag > threshold + f.threshold && f.interacts)
                    {
                        f.InterFieldInteractions(activePoint, this);
                    }
                }
            }

            if (activePoint.vector.x != 0 || activePoint.vector.x != 0) {
                for (int c = -1; c <= 1; c++)
                {
                    for (int b = -1; b <= 1; b++)
                    {
                        if (c != 0 || b != 0)
                        {
                            if (activePoint.x + c > 0 && activePoint.x + c < Program.width - 1
                                && activePoint.y + b > 0 && activePoint.y + b < Program.height - 1)
                            {
                                Point p = points[activePoint.x + c, activePoint.y + b];
                                if (p != null)
                                {
                                    if (p.vector.mag > 0)
                                    {
                                        int difX = (activePoint.vector.x + p.vector.x / 2) / 4;
                                        int difY = (activePoint.vector.y + p.vector.y / 2) / 4;

                                        p.vector.x += difX;
                                        p.vector.y += difY;
                                        activePoint.vector.x -= difX;
                                        activePoint.vector.y -= difY;
                                    }
                                }
                            }
                        }
                    }
                }
            }


            //int activeNeighbors = activePoint.CheckNeighbors(this);

            if (activePoint.vector.mag > tension * 2 && activePoint.vector.mag > threshold && hasHighParticles)
            {
                HighParticle(activePoint);
            }
            else if (activePoint.vector.mag > threshold)
            {
                MidParticle(activePoint);
            }
            else
            {
                LowParticle(activePoint, x, y);
            }
        }

        public void Initiate(int width, int height, float startMag)
        {
            points = new Point[width, height];

            for (int x = 1; x < width; x++)
            {
                for (int y = 1; y < height; y++)
                {
                    points[x, y] = new Point()
                    {
                        x = x,
                        y = y,
                        vector = new Vector()
                        {
                            x = 0,
                            y = 0,
                            mag = startMag
                        }
                    };
                }
            }
        }

        public void HighParticle (Point activePoint)
        {
            if (activePoint.vector.x != 0 && activePoint.hasMoved == false || activePoint.vector.y != 0 && activePoint.hasMoved == false)
            {
                activePoint.hasMoved = true;

                if (activePoint.x + activePoint.vector.x <= 0 || activePoint.x + activePoint.vector.x > Program.width - 1) { activePoint.vector.x = activePoint.vector.x * -1; }
                if (activePoint.y + activePoint.vector.y <= 0 || activePoint.y + activePoint.vector.y > Program.height - 1) { activePoint.vector.y = activePoint.vector.y * -1; }
                
                if (activePoint.x + activePoint.vector.x <= 0 || activePoint.x + activePoint.vector.x > Program.width - 1) { activePoint.vector.x = 0; }
                if (activePoint.y + activePoint.vector.y <= 0 || activePoint.y + activePoint.vector.y > Program.height - 1) { activePoint.vector.y = 0; }

                Point p = this.points[activePoint.x + activePoint.vector.x, activePoint.y + activePoint.vector.y];
                if (p != null)
                {

                    if (p.hasMoved == false && p != activePoint)
                    {

                        p.vector.mag += activePoint.vector.mag;
                        p.vector.x += activePoint.vector.x;
                        p.vector.y += activePoint.vector.y;

                        p.hasMoved = true;

                        activePoint.vector.mag = 0;
                        activePoint.vector.x = 0;
                        activePoint.vector.y = 0;
                    }
                }
            }
        }

        public void MidParticle(Point activePoint)
        {
            if (activePoint.vector.x != 0 && activePoint.hasMoved == false || activePoint.vector.y != 0 && activePoint.hasMoved == false)
            {
                activePoint.hasMoved = true;

                if (activePoint.x + activePoint.vector.x <= 0 || activePoint.x + activePoint.vector.x > Program.width - 1) { activePoint.vector.x = activePoint.vector.x * -1; }
                if (activePoint.y + activePoint.vector.y <= 0 || activePoint.y + activePoint.vector.y > Program.height - 1) { activePoint.vector.y = activePoint.vector.y * -1; }

                if (activePoint.x + activePoint.vector.x <= 0 || activePoint.x + activePoint.vector.x > Program.width - 1) { activePoint.vector.x = 0; }
                if (activePoint.y + activePoint.vector.y <= 0 || activePoint.y + activePoint.vector.y > Program.height - 1) { activePoint.vector.y = 0; }

                Point p = this.points[activePoint.x + activePoint.vector.x, activePoint.y + activePoint.vector.y];
                if (p != null)
                {

                    if (p.hasMoved == false && p != activePoint)
                    {

                        p.vector.mag += activePoint.vector.mag;
                        p.vector.x += activePoint.vector.x;
                        p.vector.y += activePoint.vector.y;

                        p.hasMoved = true;

                        activePoint.vector.mag = 0;
                        activePoint.vector.x = 0;
                        activePoint.vector.y = 0;
                    }
                }
            }

            LowParticle(activePoint, activePoint.x, activePoint.y);
        }

        public void LowParticle(Point activePoint, int x, int y)
        {
            float diffuse = (activePoint.vector.mag / 8) * (1 / tension);

            for (int i = -1; i <= 1; i++)
            {
                for (int k = -1; k <= 1; k++)
                {
                    if (i != 0 || k != 0)
                    {
                        if (x + i > 0 && y + k > 0
                            && x + i < Program.width
                            && y + k < Program.height)
                        {
                            if (diffuse > 0)
                            {

                                Point p = this.points[x + i, y + k];

                                p.vector.mag += diffuse;

                                activePoint.vector.mag -= diffuse;
                            }
                        }
                    }
                }
            }
        }
    }

    public class Point
    {
        public int x;
        public int y;

        public Vector vector;
        public bool hasMoved;

        public int CheckNeighbors(Field f)
        {
            int activeNeighbors = 0;

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (this.x + x > 0 && this.x + x < Program.width
                        && this.y + y > 0 && this.y + y < Program.height)
                    {
                        Point p = f.points[this.x + x, this.y + y];

                        if (p != null)
                        {
                            if (p.vector.mag > 0)
                            {
                                activeNeighbors++;
                            }
                        }
                    }
                }
            }

            return activeNeighbors;
        }
    }

    public class Vector
    {
        public int x;
        public int y;

        public float mag;
        //public float pMag;
    }
}