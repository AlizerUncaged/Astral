using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astral
{
    public interface IImageCompressor
    {
        public Models.ScreenConfig ScreenConfig { get; }

        /// <summary>
        /// Compresses an image retaining its tag.
        /// </summary>
        public Bitmap Compress(Bitmap original);
    }
}
