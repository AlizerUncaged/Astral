using Astral.Models;
using Pastel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static Astral.Monitor.ScreenInfo;

namespace Astral.Monitor
{
    public class ScreenGrab : IConfiguredService<ScreenConfig>, IService
    {
        public ScreenConfig Configuration { get; }

        private bool uncappedFps = false;
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

            var msWait = 1000 / Configuration.Fps;

            bm = new Bitmap(scInfo.Bounds.Width,
                scInfo.Bounds.Height);

            uncappedFps = Configuration.Fps < 1;

            Console.WriteLine($"Screenshot wait time : {$"{msWait}".Pastel(Color.LightCyan)}ms " +
                $"or {$"{Configuration.Fps}".Pastel(Color.LightCyan)} fps.".Pastel(Color.DarkGray));

            if (!uncappedFps)
                timer = new PeriodicTimer(TimeSpan.FromMilliseconds(msWait));

            // Quick fix if the user did a stupid.
            Configuration.Downscale = Configuration.Downscale > 1 ?
                1 : Configuration.Downscale;

            Console.WriteLine($"Screen monitor configuration =>{Environment.NewLine}{configuration}");

            if (Configuration.Fps < 1)
                Console.WriteLine($"! {"Screenshot Fps is uncapped.".Pastel(Color.LightCoral)} We'll be " +
                    $"utilizing the entire GPU which might cause lag on the game.");
        }

        /// <summary>
        /// Event called whenever the screenshot occured.
        /// </summary>
        public event EventHandler<Bitmap>? Screenshot;

        /// <summary>
        /// Event whenever we're about to screenshot.
        /// </summary>
        public event EventHandler? ScreenshotStarted;

        public void Stop() => doScreenshot = false;

        public async Task StartPeriodicScreenshotAsync() =>
            await Task.Run(async () =>
            {
                while (doScreenshot)
                {
                    if (!uncappedFps) await timer.WaitForNextTickAsync();

                    ScreenshotStarted?.Invoke(this, new EventArgs());

                    Screenshot?.Invoke(this, GetSreenshot());
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
            g.ScaleTransform(Configuration.Downscale, Configuration.Downscale);
            return bm;
        }
    }
}
