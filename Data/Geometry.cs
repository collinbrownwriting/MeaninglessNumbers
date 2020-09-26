using System.Runtime.InteropServices;

namespace Meaningless_Numbers {
    public class Geometry
    {
        [StructLayout(LayoutKind.Sequential)]
        public class Coords
        {
            public int x, y;
            public Coords(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SmallRect
        {
            public short Left;
            public short Top;
            public short Right;
            public short Bottom;
        }
    }
}
