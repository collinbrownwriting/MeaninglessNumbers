using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Meaningless_Numbers
{
    class Program
    {
        [DllImport("kernel32.dll", EntryPoint = "GetStdHandle", SetLastError = true, CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern int GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll", EntryPoint = "SetConsoleMode", SetLastError = true, CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern bool SetConsoleMode(int hConsoleHandle, int dwMode);
        private const int STD_OUTPUT_HANDLE = -11;

        [Flags]
        public enum OutputModeFlags
        {
            ENABLE_PROCESSED_OUTPUT = 0x01,
            ENABLE_WRAP_AT_EOL_OUTPUT = 0x02
        }

        public static int width = 100;
        public static int height = 36;

        public static int screen_width = 220;
        public static int screen_height = 62;

        public static Engine engine;
        public static GameMap gameMap;
        public static InputHandler inputHandler;
        public static RootConsole rootConsole;

        public static List<Field> fields;
        public static Field gravityField;
        public static Field electromagField;
        public static Field higgsField;
        public static Field strongField;
        public static Field weakField;

        static void Main()
        {
            int hConsoleOutput = GetStdHandle(STD_OUTPUT_HANDLE);
            SetConsoleMode(hConsoleOutput, (int)OutputModeFlags.ENABLE_PROCESSED_OUTPUT);

            rootConsole = new RootConsole(screen_width, screen_height);
            gameMap = new GameMap(width, height);
            inputHandler = new InputHandler();
            engine = new Engine();

            gravityField = new Field()
            {
                interacts = false,
                threshold = 1,
                tension = 1f,
                hasHighParticles = false
            };
            electromagField = new Field()
            {
                interacts = false,
                threshold = 10,
                tension = 5f,
                hasHighParticles = true
            };
            higgsField = new Field()
            {
                interacts = false,
                threshold = 10,
                tension = 20f,
                hasHighParticles = true
            };
            strongField = new Field()
            {
                interacts = false,
                threshold = 15,
                tension = 1f,
                hasHighParticles = false
            };
            weakField = new Field()
            {
                interacts = false,
                threshold = 1,
                tension = 15f,
                hasHighParticles = false
            };


            fields = new List<Field>()
            {
                gravityField,
                electromagField,
                higgsField,
                strongField,
                weakField
            };

            gravityField.Initiate(width, height, 0f);
            electromagField.Initiate(width, height, 0);
            higgsField.Initiate(width, height, 0);
            strongField.Initiate(width, height, 0f);
            weakField.Initiate(width, height, 0f);

            while (true)
            {
                engine.Update();
            }
        }
    }
}
