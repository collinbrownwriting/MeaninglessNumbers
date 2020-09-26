using System;
using System.ComponentModel;

namespace Meaningless_Numbers
{
	public class GameMap
	{
		public int width;
		public int height;
		public Tile[,] tiles;

		public RootConsole rootConsole;

		public GameMap(int width, int height)
        {
			instance = this;

			Console.Clear();

			this.width = width;
			this.height = height;
			this.tiles = new Tile[width, height];
			GenerateLayer();
        }

		public void GenerateLayer ()
        {
			for (int x = 1; x < width; x++)
            {
				for (int y = 1; y < height; y ++)
                {
					Tile newTile = new Tile()
					{
						coords = new Geometry.Coords(x, y),
						character = '.',
						foreColor = ConsoleColor.Black,
						backColor = ConsoleColor.DarkGray
					};

					this.tiles[x, y] = newTile;
                }
            }
			GenerateMap();
        }

		public void GenerateMap()
        {
			foreach (Tile t in tiles)
            {
				if (t != null)
                {
					Console.SetCursorPosition(t.coords.x, t.coords.y);
					Console.BackgroundColor = t.backColor;
					Console.ForegroundColor = t.foreColor;
					Console.Write(t.character);
                }
            }
        }

		public class ScreenBuffer
        {
			public static char[,] screenBufferArray = new char[Program.gameMap.width, Program.gameMap.height];
			public static string screenBuffer;
			public static Char[] arr;
			public static int i = 0;

			public static void Draw(string text, int x, int y)
            {
				arr = text.ToCharArray(0, text.Length);
				i = 0;
				foreach (char c in arr)
                {
					screenBufferArray[x + i, y] = c;
					i++;
                }
            }

			public static void DrawScreen()
            {
				screenBuffer = "";
				for (int iy = 0; iy < Program.gameMap.height; iy++)
                {
					for (int ix = 0; ix < Program.gameMap.width; ix++)
                    {
						screenBuffer += screenBufferArray[ix, iy];
                    }
                }
				Console.SetCursorPosition(0, 0);
				Console.Write(screenBuffer);
				screenBufferArray = new char[Program.gameMap.width, Program.gameMap.height];
            }
        }

		public void RenderMap()
        {
			foreach (Tile t in this.tiles)
            {
				if (t != null)
                {
					ScreenBuffer.Draw(t.character.ToString(), t.coords.x, t.coords.y);
                }
            }

			ScreenBuffer.DrawScreen();
        }

		public class Tile
        {
			public Geometry.Coords coords;
			public char character;
			
			public ConsoleColor foreColor;
			public ConsoleColor backColor;
        }

		public static GameMap instance;

	}
}