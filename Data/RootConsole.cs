using System;
using System.Runtime.CompilerServices;

namespace Meaningless_Numbers
{
	public class RootConsole
	{
		public RootConsole(int width, int height)
		{
			//Console.SetWindowSize(width, height);
			Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
		}

		public void WriteAt(char character, Geometry.Coords c, ConsoleColor foreColor, ConsoleColor backColor)
        {
			Console.SetCursorPosition(c.x, c.y);
			Console.ForegroundColor = foreColor;
			Console.BackgroundColor = backColor;
			Console.Write(character);
        }
	}
}