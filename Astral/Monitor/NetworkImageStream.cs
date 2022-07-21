using Astral.Models;
using Astral.Networking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astral.Monitor
{
    public class NetworkImageStream : IConfiguredService<ScreenConfig>, IInputImage, IService
    {
        private readonly NetListener websocket;

        public ScreenConfig Configuration { get; }

        public event EventHandler<Bitmap>? InputRendered;
        public event EventHandler? InputStarting;
        private readonly IImageCompressor imageCompressor;

        public NetworkImageStream(
            NetListener websocket,
            ScreenConfig screenConfig,
            IImageCompressor imageCompressor)
        {
            Configuration = screenConfig;
            this.imageCompressor = imageCompressor;
            this.websocket = websocket;

            websocket.ImageReceived += ImageOverNetworkReceived;
        }

        private void ImageOverNetworkReceived(object? sender, Bitmap e) =>
            InputRendered?.Invoke(sender, imageCompressor.Compress(e));


        public Task StartAsync()
        {
            websocket.StartAcceptingClients();
            return Task.CompletedTask;
        }

        public void Stop()
        {
            websocket.Stop();
        }
    }
}
