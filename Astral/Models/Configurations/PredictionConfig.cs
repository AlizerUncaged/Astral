using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astral.Models.Configurations
{
    public class PredictionConfig : IConfig
    {
        /// <summary>
        /// The minimum level of confidence of the AI's
        /// output to be used.
        /// </summary>
        public float MinimumScore { get; set; } = 0.5f;
    }
}
