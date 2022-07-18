using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astral.Monitor
{
    /// <summary>
    /// Configuration for the screen grabber with default values.
    /// </summary>
    public class ScreenConfig : IConfig
    {
        public Screen Screen { get; set; } = Screen.PrimaryScreen;

        public int Fps { get; set; } = 15;

        public int Downscale { get; set; } = 2;

        public override string ToString() =>
            $"Current Screen : {Screen.DeviceName}{Environment.NewLine}" +
            $"Screenshot FPS : {Fps}{Environment.NewLine}" +
            $"Screenshot Downscale : {Downscale}";
    }
}
