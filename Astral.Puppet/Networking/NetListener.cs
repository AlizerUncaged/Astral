using Astral.Curses;
using Astral.Models;
using Astral.Puppet.Input;
using Astral.Puppet.Models;
using LiteNetLib;
using LiteNetLib.Utils;
using Serilog;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astral.Puppet.Networking
{
    // Client
    public class NetListener : IService
    {
        private readonly NetworkLock networkLock;
        private readonly NetworkConfig networkConfig;
        private readonly ILogger logger;
        private readonly ActiveWindowGrab activeWindowGrab;
        private EventBasedNetListener listener;
        private NetManager me;
        private NetPeer server;

        private bool keepPolling = true;

        private readonly NetPacketProcessor netPacketProcessor =
            new NetPacketProcessor();

        public NetListener(Models.NetworkLock networkLock,
            Astral.Models.NetworkConfig networkConfig,
            Input.ActiveWindowGrab activeWindowGrab,
            ILogger logger)
        {
            this.networkLock = networkLock;
            this.networkConfig = networkConfig;
            this.logger = logger;
            this.activeWindowGrab = activeWindowGrab;
            logger.Debug("Client listener started...");

            listener = new EventBasedNetListener();
            me = new NetManager(listener);


            listener.NetworkReceiveEvent += PacketReceive;
            listener.PeerConnectedEvent += ConnectedToServer;

            netPacketProcessor
                .SubscribeReusable<NetworkObjectBounds, NetPeer>(MouseInputReceived);

            netPacketProcessor
                .SubscribeReusable<NetworkAcknowledge, NetPeer>(AcknowledgeReceived);

        }

        private void ConnectedToServer(NetPeer peer)
        {
            activeWindowGrab.InputRendered += ScreenshotRendered;
            logger.Information($"Server accepted connection at {peer.EndPoint}");
        }

        private ImageConverter converter = new ImageConverter();
        private void ScreenshotRendered(object? sender, Bitmap e)
        {
            var bitmapByteArray = (byte[])converter.ConvertTo(e, typeof(byte[]))!;
            var networkImageDataPacket = new NetworkImageData()
            {
                ImageData = bitmapByteArray
            };

            server.Send(netPacketProcessor.Write(networkImageDataPacket),
                DeliveryMethod.ReliableOrdered);

        }

        private void MouseInputReceived(NetworkObjectBounds mouseData, NetPeer netPeer)
        {
            logger.Debug($"Received mouse position from main server.");
            MousePositionChanged?.Invoke(this, mouseData);
        }

        private void AcknowledgeReceived(NetworkAcknowledge mouseData, NetPeer netPeer)
        {
            // If ack or any.
            if (networkLock.Lock.CurrentCount <
                    Models.NetworkLock.MaxSimultaneousScreenshotSend)
            {

                // logger.Debug($"Acknowledge received, releasing screenshot lock.");
                networkLock.Lock.Release();
            }
        }

        public event EventHandler<NetworkObjectBounds> MousePositionChanged;

        private void PacketReceive(NetPeer peer, NetPacketReader reader, DeliveryMethod deliveryMethod) =>
            netPacketProcessor.ReadAllPackets(reader, peer);

        public void Stop()
        {
            keepPolling = false;
            me.Stop();
        }

        public void StartListening()
        {
            me.Start();
            server = me.Connect("192.168.254.107",
                networkConfig.ServerPort,
                networkConfig.Password);

            logger.Information($"Client listener connected at " +
                $"{networkConfig.ServerHost}");

            // Measure ping.
            _ = Task.Run(async () =>
            {
                var oneSecondPeriondTimer =
                    new PeriodicTimer(TimeSpan.FromSeconds(1));

                while (await oneSecondPeriondTimer.WaitForNextTickAsync())
                    logger.Debug($"Server latency: {server.Ping}ms");
            });

            _ = Task.Run(() =>
            {
                while (keepPolling)
                    me.PollEvents();
            });
        }
    }
}
