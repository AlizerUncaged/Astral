using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astral.Models
{
    public class BitmapWithPeerData
    {
        public Networking.NetClient NetClient { get; set; }

        public Models.NetworkImageData NetworkImageData { get; set; }
    }
}
