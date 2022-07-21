using Astral.Models;
using Astral.Utilities;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astral.Monitor
{
    public class ActiveWindowGrab : IConfiguredService<ScreenConfig>, IInputImage, IService
    {
        public ScreenConfig Configuration { get; }

        private bool doScreenshot = true;

        public event EventHandler? InputStarting;
        public event EventHandler<Bitmap>? InputRendered;

        private PeriodicTimer timer;
        private readonly ForegroundWindow foregroundWindow;
        private readonly IImageCompressor imageCompressor;
        private readonly ILogger logger;

        public ActiveWindowGrab(ScreenConfig configuration,
            ForegroundWindow foregroundWindow,
            IImageCompressor imageCompressor, ILogger logger)
        {
            Configuration = configuration;
            this.foregroundWindow = foregroundWindow;
            this.imageCompressor = imageCompressor;
            this.logger = logger;
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

                    InputStarting?.Invoke(this, EventArgs.Empty);

                    var activeWindowBounds =
                        foregroundWindow.GetForegroundWindowBounds();

                    // logger.Debug($"Active window size : {activeWindowBounds.Size}");

                    var startingPoint = new Point(activeWindowBounds.X, activeWindowBounds.Y);

                    // Make sure it's a valid screenshot.
                    if (activeWindowBounds is { Width: < 2, Height: < 2 })
                        continue;

                    var rawScreenshot = new Bitmap(activeWindowBounds.Width, activeWindowBounds.Height);
                    var g = Graphics.FromImage(rawScreenshot);
                    g.CopyFromScreen(startingPoint, Point.Empty, activeWindowBounds.Size);

                    // Clone and resize the bitmap to downscale only if needed.
                    if (Configuration.Downscale != 1)
                        InputRendered?.Invoke(this, imageCompressor.Compress(rawScreenshot));

                    // Send as is if no downscale required.
                    else
                        InputRendered?.Invoke(this, rawScreenshot);
                }


            });

        public void Stop() => doScreenshot = false;
    }
}
