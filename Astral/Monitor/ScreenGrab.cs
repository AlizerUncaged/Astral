using Autofac;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static Astral.Monitor.ScreenInfo;
using static SharpCV.Binding;

namespace Astral.Monitor
{
    public class ScreenGrab : IConfiguredService<ScreenConfig>, IService
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

            timer = new PeriodicTimer(TimeSpan.FromMilliseconds(1000 / Configuration.Fps));

            var scInfo = From(Configuration.Screen);

            bm = new Bitmap(scInfo.PhysicalBounds.Width,
                scInfo.PhysicalBounds.Height);

            Console.WriteLine($"Screen monitor configuration =>{Environment.NewLine}{configuration}");
        }

        public event EventHandler<SharpCV.Mat>? Screenshot;

        public async Task StartPeriodicScreenshotAsync() =>
            await Task.Run(async () =>
            {
                while (doScreenshot && await timer.WaitForNextTickAsync())
                {
                    var screenshot = GetSreenshot().ToMat();

                    using (var cv2Screenshot = new SharpCV.Mat(screenshot.CvPtr)) // Convert to SharpCV Mat object.
                    {
                        using (var resized = cv2.resize(cv2Screenshot,
                                ((int)cv2Screenshot.shape[1] / Configuration.Downscale, (int)cv2Screenshot.shape[0] / Configuration.Downscale)))

                        {
                            if (resized.size > 0 && resized.Channels > 0)
                                Screenshot?.Invoke(this, resized);
                        }
                    }
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

            return bm;
        }
    }
}
