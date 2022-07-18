using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static Astral.ScreenInfo;
using static SharpCV.Binding;

namespace Astral
{
    public class ScreenGrab
    {
        private readonly int scale;
        private readonly int delay;
        private bool doScreenshot = true;
        private PeriodicTimer timer;

        /// <summary>
        /// Screenshot utility.
        /// </summary>
        /// <param name="downScale">Multiple size of the screenshot image to scale on, 2 will scale the image down to 50%.</param>
        /// <param name="fps">Screenshot fps, setting this to 20 will make the program capture 20 screenshots per second, still depends on hardware speed.</param>
        public ScreenGrab(int downScale, int fps, Screen screen)
        {
            this.scale = downScale;
            this.delay = 1000 / fps;
            this.timer = new PeriodicTimer(TimeSpan.FromMilliseconds(delay));

            var scInfo = ScreenInfo.From(screen);

            bm = new Bitmap(scInfo.PhysicalBounds.Width,
                scInfo.PhysicalBounds.Height);
        }

        public event EventHandler<SharpCV.Mat>? Screenshot;

        public async Task StartPeriodicScreenshotAsync()
        {
            while (doScreenshot && await timer.WaitForNextTickAsync())
            {
                var screenshot = GetSreenshot();

                using (var cv2Screenshot = new SharpCV.Mat(screenshot.ToMat().CvPtr)) // Convert to SharpCV Mat object.
                {
                    using (var resized = cv2.resize(cv2Screenshot,
                            ((int)cv2Screenshot.shape[1] / scale, (int)cv2Screenshot.shape[0] / scale)))
                    {
                        Screenshot?.Invoke(this, resized);
                    }
                }
            }
        }

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
