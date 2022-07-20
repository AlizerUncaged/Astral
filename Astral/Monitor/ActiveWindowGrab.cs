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

        public event EventHandler? ScreenshotStarting;
        public event EventHandler<Bitmap>? ScreenshotRendered;

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
                    if (!Configuration.IsUncapped && timer is { })
                        await timer.WaitForNextTickAsync();

                    ScreenshotStarting?.Invoke(this, null);

                    var activeWindowBounds =
                        foregroundWindow.GetForegroundWindowBounds();

                    var startingPoint = new Point(activeWindowBounds.X, activeWindowBounds.Y);

                    // Make sure it's a valid screenshot.
                    if (activeWindowBounds is { Width: > 2, Height: > 2 })
                        using (Bitmap bitmap = new Bitmap(activeWindowBounds.Width, activeWindowBounds.Height))
                        {
                            using (Graphics g = Graphics.FromImage(bitmap))
                            {
                                g.CopyFromScreen(startingPoint, Point.Empty, activeWindowBounds.Size);

                                // Clone and resize the bitmap to downscale only if needed.
                                if (Configuration.Downscale != 1)
                                    using (var resized = new Bitmap(bitmap,
                                            new Size((int)(bitmap.Size.Width * Configuration.Downscale),
                                                        (int)(bitmap.Size.Height * Configuration.Downscale))))
                                    {
                                        ScreenshotRendered?.Invoke(this, resized);
                                    }

                                // Send as is if no downscale required.
                                else
                                    ScreenshotRendered?.Invoke(this, bitmap);
                            }
                        }
                }
            });

        public void Stop() => doScreenshot = false;
    }
}
