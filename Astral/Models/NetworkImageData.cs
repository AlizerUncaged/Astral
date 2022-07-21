using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astral.Models
{
    /// <summary>
    /// Packet for NetListener, it only supports primitive types.
    /// </summary>
    public class NetworkImageData
    {
        public int WindowsLocationX { get; set; }
        public int WindowsLocationY { get; set; }

        public int ObjectLocationX { get; set; }
        public int ObjectLocationY { get; set; }

        public int BoxSizeWidth { get; set; }
        public int BoxSizeHeight { get; set; }

        public byte[] ImageData { get; set; }
    }
}
