using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astral.Models.Configurations
{
    public class ModelConfig : IConfig
    {
        public string? WeightsFilepath { get; set; } = "./Dependencies/YoloV4/CSGO/csgo.weights";

        public string? CfgFilepath { get; set; } = "./Dependencies/YoloV4/CSGO/csgo.cfg";

        public string? NamesFilepath { get; set; } = "./Dependencies/YoloV4/CSGO/csgo.names";
    }
}
