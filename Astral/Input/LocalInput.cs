using Astral.Curses;
using Astral.Models;
using Astral.Models.Configurations;
using Astral.Utilities;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astral.Input
{
    public class LocalInput : IService, IPredictionConsumer,
        IConfiguredService<PredictionConfig>
    {
        private readonly ILogger logger;
        private readonly PositionCalculator positionCalculator;
        private readonly ForegroundWindow foregroundWindow;
        private readonly EntityPicker entityPicker;
        private readonly Hotkeys hotkeys;
        private readonly LocalMouseControl mouseControl;

        public LocalInput(IDetectorService detectorService,
            ILogger logger,
            PredictionConfig predictionConfig,
            PositionCalculator positionCalculator,
            ForegroundWindow foregroundWindow,
            EntityPicker entityPicker,
            Hotkeys hotkeys,
            LocalMouseControl mouseControl)
        {
            this.Configuration = predictionConfig;
            this.positionCalculator = positionCalculator;
            this.foregroundWindow = foregroundWindow;
            this.entityPicker = entityPicker;
            this.hotkeys = hotkeys;
            this.mouseControl = mouseControl;
            this.logger = logger;

            detectorService.PredictionReceived += PredictionReceived;
        }

        public PredictionConfig Configuration { get; }

        private void PredictionReceived(object? sender, IEnumerable<PredictionResult> e)
        {
            var highConfidenceObjects = e.Where(x => x.Score > Configuration.MinimumScore);

            // Find via label index.
            var persons = highConfidenceObjects.Where(x => x.LabelIndex is 1); // 1 = Person

            // Find via label name.
            persons = persons.Any() ? persons : highConfidenceObjects; //.Where(x => string.Equals(x.Label, "enemy", StringComparison.OrdinalIgnoreCase));

            if (persons.Any())
            {
                var nearestEntity = entityPicker.GetCurrentEntityFocus(e)!;

                var currentActiveWindowLocation = foregroundWindow
                    .GetForegroundWindowBounds().Location;

                var objectLocation = positionCalculator
                    .RecalculateObjectPosition(currentActiveWindowLocation,
                        Point.Round(nearestEntity.Location),
                        Size.Round(nearestEntity.Size));

                mouseControl.MouseLocation = Point.Round(objectLocation);

                logger.Debug($"Nearest entity found at " +
                    $"{objectLocation} " +
                    $"located on desktop.");
            }
        }
    }
}
