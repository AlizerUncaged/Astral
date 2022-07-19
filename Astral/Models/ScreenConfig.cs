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
        public int Fps { get; set; } = 20;

        /// <summary>
        /// The percentage of the screenshot to downscale.
        /// </summary>
        public float Downscale { get; set; } = 0.5f; // 0.5 is 50% of the original image's size.

        public override string ToString() =>
            $"Current Screen : {$"{Screen.DeviceName}".Pastel(Color.LightCyan)}{Environment.NewLine}" +
            $"Screenshot FPS : {$"{Fps}".Pastel(Color.LightCyan)}{Environment.NewLine}" +
            $"Screenshot Downscale : {$"{Downscale}".Pastel(Color.LightCyan)}".Pastel(Color.DarkGray);
    }
}
