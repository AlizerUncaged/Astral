using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astral.Models.Configurations
{
    public enum CompressorOptions
    {
        ServerSide,
        ClientSide
    }

    /// <summary>
    /// Configuration for the screen grabber with default values.
    /// </summary>
    public class ScreenConfig : IConfig
    {
        /// <summary>
        /// The screen to use if you're using a screen grabber for
        /// an entire monitor.
        /// </summary>
        public Screen Screen { get; set; } = Screen.PrimaryScreen;

        /// <summary>
        /// The maximum predictions per second. If the value is below
        /// or equals to 0, unlimited predictions per second.
        /// </summary>
        // It's best to set this to prevent GPU overheat.
        public int Fps { get; set; } = 30;

        /// <summary>
        /// If input is sent over network, this option dictates
        /// where the compression will occur.
        /// </summary>
        public CompressorOptions CompressionLocation { get; set; } =
            CompressorOptions.ClientSide;

        /// <summary>
        /// The percentage of the screenshot to downscale.
        /// The lower the image resolution the faster the prediction
        /// but also inaccurate.
        /// </summary>
        public float Downscale { get; set; } = 1f;

        public int ScreenshotWaitTime => 1000 / this.Fps;

        public bool IsUncapped => Fps <= 0;

        public override string ToString() =>
            $"Current Screen: {Screen.DeviceName}{Environment.NewLine}" +
            $"Screenshot FPS: {Fps}{Environment.NewLine}" +
            $"Screenshot Downscale: {Downscale * 100f}%";
    }
}
