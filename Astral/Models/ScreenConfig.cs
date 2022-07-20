using Pastel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astral.Models
{
    /// <summary>
    /// Configuration for the screen grabber with default values.
    /// </summary>
    public class ScreenConfig : IConfig
    {
        public Screen Screen { get; set; } = Screen.PrimaryScreen;

        /// <summary>
        /// The maximum predictions per second. If the value is below
        /// or equals to 0, unlimited predictions per second.
        /// </summary>
        // It's best to set this to prevent GPU overheat.
        public int Fps { get; set; } = 20;

        /// <summary>
        /// The percentage of the screenshot to downscale.
        /// The lower the image resolution the faster the prediction
        /// but also inaccurate.
        /// </summary>
        public float Downscale { get; set; } = 0.5f;

        public int ScreenshotWaitTime => 1000 / this.Fps;

        public bool IsUncapped => Fps <= 0;

        public override string ToString() =>
            $"Current Screen : {$"{Screen.DeviceName}".Pastel(Color.LightCyan)}{Environment.NewLine}" +
            $"Screenshot FPS : {$"{Fps}".Pastel(Color.LightCyan)}{Environment.NewLine}" +
            $"Screenshot Downscale : {$"{Downscale * 100f}%".Pastel(Color.LightCyan)}".Pastel(Color.DarkGray);
    }
}
