using Astral.Curses;
using Astral.Detection;
using Astral.Models;
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
    public class PredictionEnumerizer : IService
    {
        private readonly IDetectorService model;
        private readonly MouseControl mouseControl;

        // Required incase the image is scaled we need
        // to recalculate the new positions.
        private readonly ScreenConfig screenConfig;

        // Required for getting the starting X and Y position
        // of the current active window.
        private readonly ForegroundWindow foregroundWindow;

        // For calculating the actual position of the object
        // in the desktop.
        private readonly PositionCalculator positionCalculator;

        private readonly ILogger logger;

        const float minimumConfidence = 0.25f;

        public PredictionEnumerizer(
            IDetectorService model,
            MouseControl mouseControl,
            ScreenConfig screenConfig,
            ForegroundWindow foregroundWindow,
            PositionCalculator positionCalculator, ILogger logger)
        {
            this.model = model;
            this.mouseControl = mouseControl;
            this.screenConfig = screenConfig;
            this.foregroundWindow = foregroundWindow;
            this.positionCalculator = positionCalculator;
            this.logger = logger;

            model.PredictionReceived += PredictionReceived;
        }

        private void PredictionReceived(object? sender, IEnumerable<Models.PredictionResult> e)
        {
            var highConfidenceObjects = e.Where(x => x.Score > minimumConfidence);

            if (!highConfidenceObjects.Any())
                return;

            logger.Debug($"{highConfidenceObjects.Count()}" +
                $" Objects: {string.Join(", ", highConfidenceObjects.Select(x => x.Label).Distinct())}");


        }
    }
}
