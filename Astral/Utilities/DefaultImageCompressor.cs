﻿using Astral.Models.Configurations;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astral.Utilities
{
    public class DefaultImageCompressor : IUtility
    {
        public DefaultImageCompressor(ScreenConfig screenConfig) =>
            ScreenConfig = screenConfig;


        public ScreenConfig ScreenConfig { get; }

        public Bitmap Compress(Bitmap original)
        {
            var st = Stopwatch.StartNew();
            var resized = new Bitmap(original,
                new Size((int)(original.Size.Width * ScreenConfig.Downscale),
                         (int)(original.Size.Height * ScreenConfig.Downscale)));

            // Retain the tag.
            resized.Tag = original.Tag;

            Console.WriteLine($"Resizing took: {st.ElapsedMilliseconds}ms");

            return resized;
        }
    }
}
