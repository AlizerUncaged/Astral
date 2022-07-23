using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astral.Models.Packets
{
    public class NetworkObjectBounds
    {
        public NetworkObjectBounds(int x, int y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public NetworkObjectBounds(Point location, Size size) :
            this(location.X, location.Y, size.Width, size.Height)
        {

        }

        public NetworkObjectBounds() : this(Point.Empty, Size.Empty)
        {

        }

        public Point Location => new Point(X, Y);

        public Size Size => new Size(Width, Height);

        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }
}
