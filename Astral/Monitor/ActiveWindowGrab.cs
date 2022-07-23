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
    public class ActiveWindowGrab : IConfiguredService<ScreenConfig>, IInputImage, IStoppable
    {
        public ScreenConfig Configuration { get; }

        public event EventHandler? InputStarting;
        public event EventHandler<Bitmap>? InputRendered;

        private PeriodicTimer? timer;
        private readonly ForegroundWindow foregroundWindow;
        private readonly AstralStatus programStatus;
        private readonly Utilities.DefaultImageCompressor imageCompressor;
        private readonly ILogger logger;

        public ActiveWindowGrab(ScreenConfig configuration,
            ForegroundWindow foregroundWindow,
            Models.AstralStatus programStatus,
            Utilities.DefaultImageCompressor imageCompressor,
            ILogger logger)
        {
            Configuration = configuration;
            this.foregroundWindow = foregroundWindow;
            this.programStatus = programStatus;
            this.imageCompressor = imageCompressor;
            this.logger = logger;

            if (!Configuration.IsUncapped)
                timer = new PeriodicTimer(TimeSpan.FromMilliseconds(Configuration.ScreenshotWaitTime));

        }
        private CancellationTokenSource screenshotWaitCancellationTokenSource =
            new CancellationTokenSource();
        public async Task StartAsync() =>
            await Task.Run(async () =>
            {
                while (!programStatus.IsClosing)
                {
                    if (!Configuration.IsUncapped &&
                        timer is { })
                        await timer.WaitForNextTickAsync(screenshotWaitCancellationTokenSource.Token);

                    InputStarting?.Invoke(this, EventArgs.Empty);

                    var activeWindowBounds = foregroundWindow.GetForegroundWindowBounds();
                    var startingPoint = new Point(activeWindowBounds.X, activeWindowBounds.Y);

                    // Make sure it's a valid screenshot.
                    // At least 2 in width and height can be read
                    // by the detectors.
                    if (activeWindowBounds is { Width: < 2, Height: < 2 })
                        continue;

                    var rawScreenshot = new Bitmap(activeWindowBounds.Width, activeWindowBounds.Height);
                    var g = Graphics.FromImage(rawScreenshot);
                    g.CopyFromScreen(startingPoint, Point.Empty, activeWindowBounds.Size);

                    // Clone and resize the bitmap to downscale only if needed.
                    if (Configuration.Downscale != 1)
                        InputRendered?.Invoke(this, imageCompressor.Compress(rawScreenshot));

                    // Send as is if no downscale required.
                    else InputRendered?.Invoke(this, rawScreenshot);
                }

                logger.Debug($"Active window grab ended.");
            });

        public void Stop()
        {
            screenshotWaitCancellationTokenSource.Cancel();
        }
    }
}
