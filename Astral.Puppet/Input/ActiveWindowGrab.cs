using Astral.Models;
using Astral.Puppet.Models;
using Astral.Utilities;
using Serilog;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astral.Puppet.Input
{
    public class ActiveWindowGrab : IService
    {
        private readonly ForegroundWindow foregroundWindow;
        private readonly Astral.Models.Configurations.ScreenConfig screenConfig;
        private readonly NetworkLock networkLock;
        private readonly DefaultImageCompressor defaultImageCompressor;
        private readonly ILogger logger;
        private readonly PeriodicTimer timer;

        public ActiveWindowGrab(
            ForegroundWindow foregroundWindow,
            Astral.Models.Configurations.ScreenConfig screenConfig, Models.NetworkLock networkLock,
            DefaultImageCompressor defaultImageCompressor, ILogger logger)
        {
            logger.Debug($"Initialized monitor...");

            this.foregroundWindow = foregroundWindow;
            this.screenConfig = screenConfig;
            this.networkLock = networkLock;
            this.defaultImageCompressor = defaultImageCompressor;
            this.logger = logger;

            if (!screenConfig.IsUncapped)
                timer = new PeriodicTimer(TimeSpan
                    .FromMilliseconds(screenConfig.ScreenshotWaitTime));
        }

        private CancellationTokenSource screenshotWaitCancellationTokenSource =
            new CancellationTokenSource();

        public async Task StartAsync()
        {
            logger.Debug($"Screenshot started...");
            while (keepTicking)
            {
                if (keepTicking && !screenConfig.IsUncapped && timer is { })
                    await timer.WaitForNextTickAsync(screenshotWaitCancellationTokenSource.Token);


                await networkLock.Lock.WaitAsync(networkLock.MaxWaitTimeout, screenshotWaitCancellationTokenSource.Token);


                var activeWindowBounds =
                    foregroundWindow.GetForegroundWindowBounds();

                // logger.Debug($"Active window size : {activeWindowBounds.Size}");

                var startingPoint = new Point(activeWindowBounds.X, activeWindowBounds.Y);

                // Make sure it's a valid screenshot.
                if (activeWindowBounds is { Width: < 2, Height: < 2 })
                    continue;

                var rawScreenshot = new Bitmap(activeWindowBounds.Width, activeWindowBounds.Height);

                // Put the Window's location in the Bitmap tag.
                rawScreenshot.Tag = startingPoint;

                var g = Graphics.FromImage(rawScreenshot);
                g.CopyFromScreen(startingPoint, Point.Empty, activeWindowBounds.Size);

                // Clone and resize the bitmap to downscale only if needed.
                if (screenConfig.Downscale != 1)
                    InputRendered?.Invoke(this, defaultImageCompressor.Compress(rawScreenshot));

                // Send as is if no downscale required.
                else
                    InputRendered?.Invoke(this, rawScreenshot);

                // logger.Debug($"Screenshot sent...");
            }

            logger.Debug($"Screenshot ended...");
        }

        public event EventHandler<Bitmap>? InputRendered;
        private bool keepTicking = true;
        public void Stop()
        {
            keepTicking = false;

            screenshotWaitCancellationTokenSource.Cancel();
            timer.Dispose();
        }
    }
}
