using Astral.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static Astral.Monitor.ScreenInfo;

namespace Astral.Monitor
{
    // TODO: Use a faster screenshoter, probably DirectX.
    // Screenshot on a 1080p monitor takes about 30ms which
    // is 3x faster than PyAutoGUI.
    public class ScreenGrab : IConfiguredService<ScreenConfig>, IMonitorService, IService
    {
        public ScreenConfig Configuration { get; }

        private bool doScreenshot = true;
        private PeriodicTimer timer;

        /// <summary>
        /// Screenshot utility.
        /// </summary>
        /// <param name="downScale">Multiple size of the screenshot image to scale on, 2 will scale the image down to 50%.</param>
        /// <param name="fps">Screenshot fps, setting this to 20 will make the program capture 20 screenshots per second, still depends on hardware speed.</param>
        public ScreenGrab(ScreenConfig configuration)
        {
            Configuration = configuration;

            var scInfo = From(Configuration.Screen);

            bm = new Bitmap(scInfo.Bounds.Width,
                scInfo.Bounds.Height);

            if (!Configuration.IsUncapped)
                timer = new PeriodicTimer(TimeSpan.FromMilliseconds(Configuration.ScreenshotWaitTime));

            Console.WriteLine($"Screen monitor configuration =>{Environment.NewLine}{configuration}");
        }

        public event EventHandler<Bitmap>? ScreenshotRendered;

        /// <summary>
        /// Event whenever we're about to screenshot.
        /// </summary>
        public event EventHandler? ScreenshotStarting;

        public void Stop() => doScreenshot = false;

        public async Task StartAsync() =>
            await Task.Run(async () =>
            {
                while (doScreenshot)
                {
                    if (!Configuration.IsUncapped && timer is { })
                        await timer.WaitForNextTickAsync();

                    ScreenshotStarting?.Invoke(this, null);

                    var screenshot = GetSreenshot();

                    ScreenshotRendered?.Invoke(this, screenshot);
                }
            });

        Bitmap bm;

        /// <summary>
        /// Screenshots the screen.
        /// </summary>
        /// <returns>Bitmap of the screenshot.</returns>
        public Bitmap GetSreenshot()
        {
            Graphics g = Graphics.FromImage(bm);
            g.CopyFromScreen(0, 0, 0, 0, bm.Size);

            var resized = new Bitmap(bm, new Size((int)(bm.Size.Width * Configuration.Downscale),
                (int)(bm.Size.Height * Configuration.Downscale)));

            return resized;
        }
    }
}
