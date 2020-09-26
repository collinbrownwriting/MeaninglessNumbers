using System;

namespace Meaningless_Numbers
{
    public class InputHandler
    {
        public void RandomStrum()
        {
            Random rand = new Random();

            int x = rand.Next(2, Program.gameMap.width - 1);
            int y = rand.Next(2, Program.gameMap.height - 1);

            int field = rand.Next(1, Program.fields.Count - 1);
            Field f = Program.fields[field];
            Point p = f.points[x, y];
            p.vector.mag = rand.Next(1, 30);
            p.vector.x = rand.Next(-1, 2);
            p.vector.y = rand.Next(-1, 2);

            for (int px = -1; px <= 1; px++)
            {
                for (int py = -1; py <= 1; py++)
                {
                    if (px == 0 || py == 0)
                    {
                        if (x + px > 0 && y + py > 0
                            && x + px < Program.width
                            && y + py < Program.height)
                        {
                            Point r = f.points[x + px, y + py];
                            r.vector.mag = rand.Next(1, 30);
                        }
                    }
                }
            }

            //int chance = rand.Next(1, 100);
            //if (chance > 95)
            //{
            //    SpecStrum(x + rand.Next(-1, 1), y + rand.Next(-1, 1), Program.fields[rand.Next(0, Program.fields.Count - 1)]);
            //}
        }

        public void SpecStrum(int x, int y, Field f)
        {
            Random rand = new Random();

            Point p = f.points[x, y];

            if (p != null)
            {

                p.vector.mag = rand.Next(5, 30);
                p.vector.x = rand.Next(-1, 1);
                p.vector.y = rand.Next(-1, 1);

                for (int px = -1; px <= 1; px++)
                {
                    for (int py = -1; py <= 1; py++)
                    {
                        if (px != 0 || py != 0)
                        {
                            if (x + px > 0 && y + py > 0
                                && x + px < Program.width
                                && y + py < Program.height)
                            {
                                Point r = f.points[x + px, y + py];
                                r.vector.mag = rand.Next(5, 30);
                                r.vector.x = rand.Next(-2, 2);
                                r.vector.y = rand.Next(-2, 2);
                            }
                        }
                    }
                }
            }
        }
    }
}
