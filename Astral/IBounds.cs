using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astral
{
    public interface IBounds
    {
        public Point Location { get; }

        public Size Size { get; }
    }
}
