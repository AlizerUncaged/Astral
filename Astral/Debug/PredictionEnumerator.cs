using Astral.Curses;
using Astral.Detection;
using Astral.Models;
using Astral.Models.Configurations;
using Astral.Utilities;
using Serilog;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yolov5Net.Scorer;

namespace Astral.Debug
{
    public class PredictionEnumerator : IService
    {

        private readonly ILogger logger;

        const float MinimumConfidence = 0.25f;

        public PredictionEnumerator(IDetectorService model, ILogger logger)
        {
            this.logger = logger;

            model.PredictionReceived += PredictionReceived;
        }

        private void PredictionReceived(object? sender, IEnumerable<PredictionResult> e)
        {
            var highConfidenceObjects = e.Where(x => x.Score > MinimumConfidence);

            if (!highConfidenceObjects.Any()) return;

            logger.Debug($"{highConfidenceObjects.Count()} " +
                $"Objects: {string.Join(", ", highConfidenceObjects.Select(x => x.Label).Distinct())}");
        }
    }
}
