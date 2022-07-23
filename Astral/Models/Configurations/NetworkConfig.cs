using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astral.Models.Configurations
{
    public class NetworkConfig : IConfig
    {
        public System.Net.IPAddress ServerHost { get; set; }
            = System.Net.IPAddress.Parse("127.0.0.1");

        public int ServerPort { get; set; } = 7220;

        public string Password { get; set; } = "astral-0";

        public int MaxConnections { get; set; } = 5;
    }
}
