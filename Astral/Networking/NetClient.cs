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

        public NetPeer NetPeer { get; set; }

        public event EventHandler<Bitmap>? ReceivedPeerImageData;


        public void Send(object obj)
        {
            NetPeer?.Send(netPacketProcessor.Write(obj), DeliveryMethod.ReliableOrdered);
        }

        public void ImageReceivedFromPeer(Bitmap imageBoundingData) =>
            ReceivedPeerImageData?.Invoke(this, imageBoundingData);

        public bool IsPeerTheSame(NetPeer peer)
        {
            return peer.EndPoint.Address == this.NetPeer.EndPoint.Address &&
                peer.EndPoint.Port == this.NetPeer.EndPoint.Port;
        }

        public override bool Equals(object? obj)
        {
            if (obj is NetClient netClient)
                return this.IsPeerTheSame(netClient.NetPeer);


            return false;
        }

        public static bool operator ==(NetClient left, NetClient right) =>
            left.Equals(right);

        public static bool operator !=(NetClient left, NetClient right) =>
            !left.Equals(right);
    }
}
