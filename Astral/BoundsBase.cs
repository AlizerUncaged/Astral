using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astral
{
    public abstract class BoundsBase : IBounds
    {
        public abstract Point Location { get; }

        public abstract Size Size { get; }

        /// <summary>
        /// Checks if a location is inside these bounds.
        /// </summary>
        public bool IsInBounds(Point target)
        {
            var maxX = Location.X + Size.Width;
            var maxY = Location.Y + Size.Height;

            return target.X > Location.X && target.X < maxX &&
                 target.Y > Location.Y && target.Y < maxY;
        }
    }
}
