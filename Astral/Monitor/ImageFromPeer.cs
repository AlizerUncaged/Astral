using Astral.Models;
using Astral.Networking;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astral.Monitor
{
    public class ImageFromPeer : IConfiguredService<ScreenConfig>,
        IInputImage,
        IStoppable
    {
        private readonly NetListener websocket;

        public ScreenConfig Configuration { get; }

        public event EventHandler<Bitmap>? InputRendered;
        public event EventHandler? InputStarting;

        private readonly Utilities.DefaultImageCompressor imageCompressor;
        private readonly ILogger logger;

        private readonly int id;

        public ImageFromPeer(
            NetListener websocket,
            ScreenConfig screenConfig,
            Utilities.DefaultImageCompressor imageCompressor,
            ILogger logger)
        {
            this.logger = logger;
            this.imageCompressor = imageCompressor;
            this.websocket = websocket;
            this.Configuration = screenConfig;

            websocket.ImageReceived += ImageOverNetworkReceived;
        }

        private void ImageOverNetworkReceived(object? sender, Bitmap e) =>
            InputRendered?.Invoke(sender, Configuration.CompressionLocation == CompressorOptions.ServerSide ?
                imageCompressor.Compress(e) : e);


        private CancellationTokenSource taskDelaySource =
            new CancellationTokenSource();

        public async Task StartAsync()
        {
            websocket.StartAcceptingClients();
            try
            {
                await Task.Delay(-1, taskDelaySource.Token);
            }
            catch (Exception)
            {

            }

            // Class dispose.
            logger.Debug($"Image from peer ended. {id}");
        }

        public void Stop()
        {
            logger.Debug($"Cancelling task delay. {id}");
            taskDelaySource.Cancel();
        }
    }
}
