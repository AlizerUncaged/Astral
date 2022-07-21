using LiteNetLib;
using LiteNetLib.Utils;
using System.Net;
using System.Net.Sockets;

namespace Astral.Puppet
{
    internal class Program
    {
        private readonly NetPacketProcessor _netPacketProcessor = new NetPacketProcessor();
        public async Task StartAsync()
        {
            // sample
            string sampleImageFile = @"D:\Users\floyd\source\repos\Astral\Assets\Sample Images\IMAGE_3.jpg";
            var bytesToSend = await File.ReadAllBytesAsync(sampleImageFile);

            EventBasedNetListener listener = new EventBasedNetListener();
            NetManager client = new NetManager(listener);
            client.Start();

            var networkConfig = new Models.NetworkConfig();
            var serverPeer =
                client.Connect("127.0.0.1", networkConfig.ServerPort, networkConfig.Password);

            await Task.Delay(1000);

            serverPeer.Send(_netPacketProcessor.Write(new Models.NetworkImageData
            {
                ImageData = bytesToSend
            }), DeliveryMethod.ReliableOrdered);

            Console.WriteLine($"Wrote {bytesToSend.Length} bytes to server...");
            while (true)
            {
                client.PollEvents();
            }
            Console.ReadKey();

        }

        static void Main(string[] args) =>
            new Program().StartAsync().GetAwaiter().GetResult();
    }
}