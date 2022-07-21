using Astral.Curses;
using Astral.Models;
using Astral.Utilities;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astral.Input
{
    public class LocalInput : IService, IInputConsumer, IConfiguredService<Models.PredictionConfig>
    {
        private readonly IDetectorService detectorService;
        private readonly ILogger logger;
        private readonly PositionCalculator positionCalculator;
        private readonly ForegroundWindow foregroundWindow;
        private readonly MouseControl mouseControl;

        public LocalInput(IDetectorService detectorService,
            ILogger logger,
            Models.PredictionConfig predictionConfig,
            Utilities.PositionCalculator positionCalculator,
            ForegroundWindow foregroundWindow,
            Curses.MouseControl mouseControl)
        {
            this.Configuration = predictionConfig;
            this.positionCalculator = positionCalculator;
            this.foregroundWindow = foregroundWindow;
            this.mouseControl = mouseControl;
            this.detectorService = detectorService;
            this.logger = logger;
            detectorService.PredictionReceived += PredictionReceived;
        }

        public PredictionConfig Configuration { get; }

        private void PredictionReceived(object? sender, IEnumerable<Models.PredictionResult> e)
        {
            var highConfidenceObjects = e.Where(x => x.Score > Configuration.MinimumScore);
            // Find via label index.
            var persons = highConfidenceObjects.Where(x => x.LabelIndex is 1); // 1 = Person

            // Find via label name.
            persons = persons.Any() ? persons : highConfidenceObjects; //.Where(x => string.Equals(x.Label, "enemy", StringComparison.OrdinalIgnoreCase));

            if (persons.Any())
            {
                var firstPerson = persons.First();
                var currentActiveWindowLocation = foregroundWindow
                    .GetForegroundWindowBounds().Location;

                var objectLocation = positionCalculator
                    .RecalculateObjectPosition(currentActiveWindowLocation,
                        Point.Round(firstPerson.Location),
                        Size.Round(firstPerson.Size));

                mouseControl.MoveMouseTo(objectLocation);

                logger.Debug($"One object found at " +
                    $"{objectLocation} " +
                    $"located on desktop.");
            }
        }
    }
}
