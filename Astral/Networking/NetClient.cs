using LiteNetLib;
using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astral.Networking
{
    public class NetClient
    {
        // Converts objects to byte[] that can be sent
        // throughout the network.
        private static readonly NetPacketProcessor netPacketProcessor =
            new NetPacketProcessor();

        // Reuse the same acknowledgement packet
        // over and over again.
        private readonly Models.NetworkAcknowledge acknowledgePacket =
            new Models.NetworkAcknowledge();

        public NetPeer? NetPeer { get; set; }

        public event EventHandler<Bitmap>? ReceivedPeerImageData;

        public void SendAcknowledge()
        {
            NetPeer?.Send(netPacketProcessor.Write(acknowledgePacket),
                DeliveryMethod.ReliableOrdered);
        }

        public void Send(Models.NetworkObjectBounds networkObjectBounds)
        {
            NetPeer?.Send(netPacketProcessor.Write(networkObjectBounds), DeliveryMethod.ReliableSequenced);
        }

        public void ImageReceivedFromPeer(Bitmap imageBoundingData) =>
            ReceivedPeerImageData?.Invoke(this, imageBoundingData);


        public override bool Equals(object? obj)
        {
            if (obj is NetClient netClient && netClient.NetPeer is { })
                return netClient.NetPeer.EndPoint.Address == this.NetPeer?.EndPoint.Address &&
                    netClient.NetPeer.EndPoint.Port == this.NetPeer.EndPoint.Port;

            return false;
        }

        public static bool operator ==(NetClient left, NetClient right) =>
            left.Equals(right);

        public static bool operator !=(NetClient left, NetClient right) =>
            !left.Equals(right);
    }
}
