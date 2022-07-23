using Astral.Models.Configurations;
using Astral.Models.Packets;
using Astral.Utilities;
using LiteNetLib;
using LiteNetLib.Utils;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Astral.Networking
{
    public class NetListener : IConfiguredService<NetworkConfig>, IStoppable
    {
        public NetworkConfig Configuration { get; }
        private readonly ILogger logger;
        private readonly SizeFormatProvider sizeFormat;
        private bool isListening = false;

        private EventBasedNetListener listener;
        private NetManager server;

        private List<NetClient> netClients =
            new List<NetClient>();

        private readonly NetPacketProcessor netPacketProcessor = new NetPacketProcessor();

        public NetListener(NetworkConfig configuration, ILogger logger, SizeFormatProvider sizeFormat)
        {
            logger.Debug($"Network object initialized!");
            Configuration = configuration;
            this.logger = logger;
            this.sizeFormat = sizeFormat;

            listener = new EventBasedNetListener();
            server = new NetManager(listener);

            listener.ConnectionRequestEvent += ConnectionRequest;
            listener.PeerConnectedEvent += ClientConnected;
            listener.PeerDisconnectedEvent += ClientDisconnected;
            listener.NetworkReceiveEvent += MessageReceived;

            netPacketProcessor
                .SubscribeReusable<NetworkImageData, NetPeer>(ImageDataReceived);
        }

        private void ClientDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
        {
            logger.Information($"Client disconnected at {peer.EndPoint} reason: {disconnectInfo.Reason}");
            switch (disconnectInfo.Reason)
            {
                case DisconnectReason.Timeout:
                    logger.Warning($"Client disconnected via timeout! Maybe their internet is bad?");
                    break;
            }
        }

        private void MessageReceived(
            NetPeer peer,
            NetPacketReader reader,
            DeliveryMethod deliveryMethod)
        {
            netPacketProcessor.ReadAllPackets(reader, peer);
        }

        private void ClientConnected(NetPeer peer)
        {
            logger.Information($"Client connected at {peer.EndPoint}");

            var peerClient = new NetClient { NetPeer = peer };

            peerClient.ReceivedPeerImageData += (p, e) =>
                ImageReceived?.Invoke(peerClient, e);

            netClients.Add(peerClient);
        }

        private void ImageDataReceived(NetworkImageData networkImageData, NetPeer netPeer)
        {
            logger.Debug($"Received image size: {networkImageData.ImageData.Length}");

            Bitmap bmp;
            using (var ms = new MemoryStream(networkImageData.ImageData))
                bmp = new Bitmap(ms);

            var peer = netClients
                .FirstOrDefault(x => x.NetPeer == netPeer);

            // Until we found a better way to send
            // the bounds data along with the bitmap
            // we'll be putting it in the Bitmap's
            // tag at the moment.
            bmp.Tag = networkImageData;

            if (peer is { })
                peer.ImageReceivedFromPeer(bmp);
            else
                logger.Warning($"An unkown peer sent an image and was discarded.");
        }

        private void ConnectionRequest(ConnectionRequest request)
        {
            logger.Debug($"Connection attempt from: {request.RemoteEndPoint.Port}");

            if (server.ConnectedPeersCount < Configuration.MaxConnections)
                request.AcceptIfKey(Configuration.Password);

            request.Reject();
        }

        public event EventHandler<Bitmap> ImageReceived;

        public bool IsListening => isListening;

        public void StartAcceptingClients()
        {
            server.Start(Configuration.ServerPort);
            logger.Information($"Server started listening at {Configuration.ServerPort}");

            isListening = true;

            _ = Task.Run(() =>
            {
                while (isListening)
                    server.PollEvents();

                logger.Debug($"Server stopped listening at {Configuration.ServerPort}");

                return Task.CompletedTask;
            });
        }

        public void Stop()
        {
            if (server.IsRunning)
                server.Stop();

            isListening = false;
        }
    }
}
