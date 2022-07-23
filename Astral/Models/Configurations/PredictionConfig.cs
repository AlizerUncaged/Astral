using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astral.Models.Configurations
{
    public class PredictionConfig : IConfig
    {
        public float MinimumScore { get; set; } = 0.25f;
    }
}
