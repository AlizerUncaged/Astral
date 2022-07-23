using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astral.Utilities
{
    public class RandomProvider : IUtility
    {
        private readonly Random random = new Random();

        public int NextInt() => random.Next();

        public long NextLong() => random.NextInt64();
    }
}
