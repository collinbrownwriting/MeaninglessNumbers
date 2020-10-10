using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Meaningless_Numbers
{
	public class Engine
	{
		public int turn;
        public float energy;

		public void Update()
		{
			turn++;
			Console.CursorVisible = false;
			Console.SetCursorPosition(0, 0);
			Console.Write("Turn: " + turn);


            energy = 0;
            foreach (Field f in Program.fields)
            {
                foreach (Point p in f.points)
                {
                    if (p != null)
                    {
                        p.hasMoved = false;
                        energy += p.vector.mag;
                    }
                }
            }

            Console.SetCursorPosition(10, 0);
            Console.Write("Energy: " + energy);

            if (turn == 1)
            {
                BigBang();
            }


            FieldInteractions();
            FieldInfluences();

            Random rand = new Random();

            //if (rand.Next(1, 100) > 90 && energy < 2000f)
            //{
            //    Program.inputHandler.RandomStrum();
            //}

            RenderWorld();

			Update();
		}

		public void FieldInteractions()
        {
            foreach (Field f in Program.fields)
            {
				if (f != null)
				{
                    for (int x = 1; x < Program.width; x++)
                    {
                        for (int y = 1; y < Program.height; y++)
                        {
                            f.HandleRipples(Program.fields, x, y);
                        }
                    }
				}
            }
        }

        public void FieldInfluences()
        {
            List<Point> gravPoints = new List<Point>();
            List<Point> massPoints = new List<Point>();
            List<Point> electroPoints = new List<Point>();
            List<Point> strongPoints = new List<Point>();

            foreach (Point m in Program.higgsField.points)
            {
                if (m != null)
                {
                    if (m.vector.mag > Program.higgsField.threshold)
                    {
                        massPoints.Add(m);
                    }
                }
            }

            foreach (Point e in Program.electromagField.points)
            {
                if (e != null)
                {
                    if (e.vector.mag > Program.electromagField.threshold)
                    {
                        massPoints.Add(e);
                    }
                }
            }

            foreach (Point g in Program.gravityField.points)
            {
                if (g != null)
                {
                    if (g.vector.mag > Program.gravityField.threshold)
                    {
                        gravPoints.Add(g);
                    }

                    Point m = Program.higgsField.points[g.x, g.y];
                    if (m.vector.mag > Program.higgsField.threshold)
                    {
                        g.vector.mag = m.vector.mag - 1;
                    }
                    else
                    {
                        g.vector.mag = 0;
                    }
                }
            }

            foreach (Point s in Program.strongField.points)
            {
                if (s != null)
                {
                    Point m = Program.higgsField.points[s.x, s.y];

                    if (m.vector.mag > Program.strongField.threshold)
                    {
                        s.vector.mag = m.vector.mag - 1;
                        strongPoints.Add(s);
                    }
                    else
                    {
                        s.vector.mag = 0;
                    }
                }
            }

            foreach (Point g in gravPoints)
            {
                if (g != null)
                {
                    foreach (Point m in massPoints)
                    {
                        if (m != null)
                        {
                            float dist = (float)Math.Sqrt(((g.x - m.x) * (g.x - m.x)) + ((g.y - m.y) * (g.y - m.y)));

                            if (dist <= 0)
                            {
                                dist = 1;
                            }

                            if (g.x > m.x + 1 && g.vector.mag > m.vector.mag && dist < g.vector.mag)
                            {
                                m.vector.x++;
                            }
                            else if (g.x < m.x - 1 && dist < g.vector.mag && g.vector.mag > m.vector.mag)
                            {
                                m.vector.x--;
                            }

                            if (g.y > m.y + 1 && g.vector.mag > m.vector.mag && dist < g.vector.mag)
                            {
                                m.vector.y++;
                            }
                            else if (g.y < m.y - 1 && dist < g.vector.mag && g.vector.mag > m.vector.mag)
                            {
                                m.vector.y--;
                            }


                            //m.vector.x = (int)(Math.Round((float)(g.x - m.vector.x / dist)));
                            //m.vector.y = (int)(Math.Round((float)(g.y - m.vector.y / dist)));
                        }
                    }

                    foreach (Point e in electroPoints)
                    {
                        float dist = (float)Math.Sqrt(((g.x - e.x) * (g.x - e.x)) + ((g.y - e.y) * (g.y - e.y)));

                        if (dist == 0)
                        {
                            dist = 1;
                        }

                        if (g.x > e.x + 1 && g.vector.mag > e.vector.mag && dist < e.vector.mag)
                        {
                            e.vector.x++;
                        }
                        else if (g.x < e.x - 1 && dist < e.vector.mag && g.vector.mag > e.vector.mag)
                        {
                            e.vector.x--;
                        }

                        if (g.y > e.y + 1 && g.vector.mag > e.vector.mag && dist < e.vector.mag)
                        {
                            e.vector.y++;
                        }
                        else if (g.x < e.x - 1 && dist < e.vector.mag && g.vector.mag > e.vector.mag)
                        {
                            e.vector.y--;
                        }

                        //e.vector.x = (int)(Math.Round((float)(g.x - e.vector.x / dist)));
                        //e.vector.y = (int)(Math.Round((float)(g.y - e.vector.y / dist)));
                    }
                }
            }

            foreach (Point s in strongPoints)
            {
                if (s != null)
                {
                    foreach (Point m in massPoints)
                    {
                        if (m != null)
                        {
                            float dist = (float)Math.Sqrt(((s.x - m.x) * (s.x - m.x)) + ((s.y - m.y) * (s.y - m.y)));

                            float dampening = s.vector.mag / m.vector.mag;

                            if (dampening > 0.8f)
                            {
                                dampening = 0.8f;
                            }

                            if (dist == 0)
                            {
                                dist = 1;
                            }

                            if (dist < 5)
                            {
                                m.vector.x += (int)(-m.vector.x * dampening);
                                m.vector.y += (int)(-m.vector.y * dampening);
                            }
                        }
                    }
                }
            }
        }

		public void RenderWorld()
        {
			for (int x = 1; x < Program.width; x++)
            {
				for (int y = 1; y < Program.height; y++)
				{
					GameMap.Tile tile = Program.gameMap.tiles[x, y];

                    bool isExcited = false;
                    float maxExcite = 0;
                    Field mostExcited = null;

                    foreach (Field f in Program.fields)
                    {
                        if (f.points[x, y].vector.mag > f.threshold && f.points[x, y].vector.mag > maxExcite)
                        {
                            maxExcite = f.points[x, y].vector.mag;
                            mostExcited = f;
                        }
                    }

                    if (maxExcite > 0 && mostExcited != null)
                    {
                        isExcited = true;

                        if (mostExcited == Program.gravityField)
                        {
                            tile.character = '1';
                            tile.foreColor = ConsoleColor.Red;
                        }
                        else if (mostExcited == Program.electromagField)
                        {
                            tile.character = '2';
                            tile.foreColor = ConsoleColor.Cyan;
                        }
                        else if (mostExcited == Program.higgsField)
                        {
                            tile.character = '3';
                            tile.foreColor = ConsoleColor.Blue;
                        }
                        else if (mostExcited == Program.strongField)
                        {
                            tile.character = '4';
                            tile.foreColor = ConsoleColor.Magenta;
                        }
                        else
                        {
                            tile.character = '5';
                            tile.foreColor = ConsoleColor.DarkGreen;
                        }
                    }

                    if (!isExcited)
                    {
                        int mostExcite = 0;
                        foreach (Field f in Program.fields)
                        {
                            if (f.points[x, y].vector.mag > mostExcite)
                            {
                                mostExcite = (int)Math.Round(f.points[x, y].vector.mag);
                            }
                        }

                        if (mostExcite > 1)
                        {
                            tile.character = (char)mostExcite;
                        }
                        else
                        {
                            tile.character = '.';
                        }
                        tile.foreColor = ConsoleColor.Black;
                    }
					
				}
			}
			//Render world.
			Program.gameMap.GenerateMap();
        }

        public void BigBang()
        {
            foreach (Field f in Program.fields)
            {
                if (f == Program.higgsField)
                {
                    Point singularity = f.points[(int)Math.Round((float)(Program.width / 2)), (int)Math.Round((float)(Program.height / 2))];
                    singularity.vector.mag += Program.width * Program.height / 30;

                    for (int i = -1; i <= 1; i++)
                    {
                        for (int k = -1; k <= 1; k++)
                        {
                            if (i != 0 || k != 0)
                            {
                                Point p = f.points[singularity.x + i, singularity.y + k];
                                p.vector.mag += Program.width * Program.height / 60;
                                p.vector.x = i;
                                p.vector.y = k;
                            }
                        }
                    }
                }
            }
        }

        public void MoveParticle(Point activePoint, Field f)
        {
            if (activePoint.vector.x != 0 && activePoint.hasMoved == false || activePoint.vector.y != 0 && activePoint.hasMoved == false)
            {
                activePoint.hasMoved = true;

                if (activePoint.x + activePoint.vector.x <= 0 || activePoint.x + activePoint.vector.x > Program.width - 1) { activePoint.vector.x = activePoint.vector.x * -1; }
                if (activePoint.y + activePoint.vector.y <= 0 || activePoint.y + activePoint.vector.y > Program.height - 1) { activePoint.vector.y = activePoint.vector.y * -1; }

                if (activePoint.x + activePoint.vector.x <= 0 || activePoint.x + activePoint.vector.x > Program.width - 1) { activePoint.vector.x = 0; }
                if (activePoint.y + activePoint.vector.y <= 0 || activePoint.y + activePoint.vector.y > Program.height - 1) { activePoint.vector.y = 0; }

                Point p = f.points[activePoint.x + activePoint.vector.x, activePoint.y + activePoint.vector.y];
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
	}
}

//if (f.CheckDifference(p.x, p.y) > f.threshold)
//{
//	GameMap.Tile activeTile = Program.gameMap.tiles[p.x, p.y];
//	if (f == Program.gravityField)
//	{
//		activeTile.character = '1';
//		activeTile.foreColor = ConsoleColor.Red;
//	}
//	else if (f == Program.electromagField)
//	{
//		activeTile.character = '2';
//		activeTile.foreColor = ConsoleColor.Cyan;
//	}
//	else if (f == Program.higgsField)
//	{
//		activeTile.character = '3';
//		activeTile.foreColor = ConsoleColor.Blue;
//	}
//	else if (f == Program.weakField)
//	{
//		activeTile.character = '4';
//		activeTile.foreColor = ConsoleColor.Yellow;
//	}
//	else if (f == Program.strongField)
//	{
//		activeTile.character = '5';
//		activeTile.foreColor = ConsoleColor.Magenta;
//	}
//	else
//	{
//		activeTile.character = '6';
//		activeTile.foreColor = ConsoleColor.DarkGreen;
//	}
//}