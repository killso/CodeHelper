using System;
using System.Collections.Generic;
using System.Text;

namespace Helper
{
    public struct Point
    {
        public Point(int x, int y) => (X, Y) = (x, y);
        public int X { get; set; }
        public int Y { get; set; }
    }
}
