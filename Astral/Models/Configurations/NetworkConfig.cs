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
            = System.Net.IPAddress.Parse("0.0.0.0");

        public int ServerPort { get; set; } = 7220;

        /// <summary>
        /// The password asked from the client before
        /// connecting to the server, set to empty or null
        /// if the server has no password.
        /// </summary>
        public string ServerPassword { get; set; } = "astral-0";

        public int MaxConnections { get; set; } = 5;
    }
}
