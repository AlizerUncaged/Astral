using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astral.Models
{
    /// <summary>
    /// Packet for mouse input.
    /// </summary>
    public class NetworkMousePosition
    {
        public NetworkMousePosition(int x, int y)
        {

        }

        public NetworkMousePosition(Point point) : this(point.X, point.Y)
        {

        }

        public NetworkMousePosition() : this(0, 0)
        {

        }


        public int X { get; set; }

        public int Y { get; set; }
    }
}
