using Astral.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astral.Utilities
{
    public class DefaultImageCompressor : IUtility, IImageCompressor
    {
        public DefaultImageCompressor(ScreenConfig screenConfig) =>
            ScreenConfig = screenConfig;


        public ScreenConfig ScreenConfig { get; }

        public Bitmap Compress(Bitmap original)
        {
            var resized = new Bitmap(original,
                new Size((int)(original.Size.Width * ScreenConfig.Downscale),
                         (int)(original.Size.Height * ScreenConfig.Downscale)));

            return resized;
        }
    }
}
