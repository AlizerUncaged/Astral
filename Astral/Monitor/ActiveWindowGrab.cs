using Astral.Models;
using Astral.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astral.Monitor
{
    public class ActiveWindowGrab : IConfiguredService<ScreenConfig>, IMonitorService, IService
    {
        public ScreenConfig Configuration { get; }

        private bool doScreenshot = true;

        public event EventHandler? ScreenshotStarted;
        public event EventHandler<Bitmap>? Screenshot;

        private PeriodicTimer timer;
        private readonly ForegroundWindow foregroundWindow;

        public ActiveWindowGrab(ScreenConfig configuration, Utilities.ForegroundWindow foregroundWindow)
        {
            Configuration = configuration;
            this.foregroundWindow = foregroundWindow;

            if (!Configuration.IsUncapped)
                timer = new PeriodicTimer(TimeSpan.FromMilliseconds(Configuration.ScreenshotWaitTime));
        }

        public async Task StartAsync() =>
            await Task.Run(async () =>
            {
                while (doScreenshot)
                {
                    if (!Configuration.IsUncapped)
                        await timer.WaitForNextTickAsync();

                    ScreenshotStarted?.Invoke(this, null);

                    var activeWindowBounds =
                        foregroundWindow.GetForegroundWindowBounds();

                    var startingPoint = new Point(activeWindowBounds.X, activeWindowBounds.Y);

                    if (activeWindowBounds.Width > 0)
                        using (Bitmap bitmap = new Bitmap(activeWindowBounds.Width, activeWindowBounds.Height))
                        {
                            using (Graphics g = Graphics.FromImage(bitmap))
                            {
                                g.CopyFromScreen(startingPoint, Point.Empty, activeWindowBounds.Size);

                                using (var resized = new Bitmap(bitmap,
                                        new Size((int)(bitmap.Size.Width * Configuration.Downscale),
                                                    (int)(bitmap.Size.Height * Configuration.Downscale))))
                                {
                                    Screenshot?.Invoke(this, resized);
                                }
                            }
                        }
                }
            });

        public void Stop() => doScreenshot = false;
    }
}
