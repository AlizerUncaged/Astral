using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astral.Models.Packets
{
    /// <summary>
    /// Packet for NetListener, it only supports primitive types.
    /// </summary>
    public class NetworkImageData
    {
        public byte[] ImageData { get; set; }
    }
}
