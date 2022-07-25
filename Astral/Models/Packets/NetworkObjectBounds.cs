using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astral.Models.Packets
{
    public class NetworkObjectBounds
    {
        public NetworkObjectBounds(int x, int y, int width, int height, int? objectIdentity = null)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            ObjectIdentity = objectIdentity;
        }

        public NetworkObjectBounds(Point location, Size size, int? objectIdentity) :
            this(location.X, location.Y, size.Width, size.Height)
        {
            ObjectIdentity = objectIdentity;
        }

        public NetworkObjectBounds() : this(Point.Empty, Size.Empty, null)
        {

        }

        public Point Location => new Point(X, Y);

        public Size Size => new Size(Width, Height);


        public float Confidence { get; set; }

        public int X { get; set; }

        public int Y { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public int? ObjectIdentity { get; set; }
    }
}
