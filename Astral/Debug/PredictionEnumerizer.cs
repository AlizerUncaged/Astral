using Astral.Curses;
using Astral.Detection;
using Astral.Models;
using Astral.Utilities;
using Pastel;
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

        const float minimumConfidence = 0.5f;

        public PredictionEnumerizer(
            IDetectorService model,
            MouseControl mouseControl,
            ScreenConfig screenConfig,
            ForegroundWindow foregroundWindow,
            PositionCalculator positionCalculator)
        {
            this.model = model;
            this.mouseControl = mouseControl;
            this.screenConfig = screenConfig;
            this.foregroundWindow = foregroundWindow;
            this.positionCalculator = positionCalculator;

            model.PredictionReceived += PredictionReceived;
        }

        private void PredictionReceived(object? sender, IEnumerable<Models.PredictionResult> e)
        {
            var highConfidenceObjects = e.Where(x => x.Score > minimumConfidence);

            if (!highConfidenceObjects.Any())
                return;

            Console.WriteLine($"{$"{highConfidenceObjects.Count()}".Pastel(Color.LightBlue)} Objects: " +
                $"{$"{string.Join(", ", highConfidenceObjects.Select(x => x.Label).Distinct())}".Pastel(Color.LightGreen)}".Pastel(Color.DarkGray));

            var persons = highConfidenceObjects.Where(x => x.LabelIndex == 1); // 1 = Person

            // Console.WriteLine($"Mouse at : {Cursor.Position}");

            persons = persons.Any() ? persons : highConfidenceObjects.Where(x => string.Equals(x.Label, "person", StringComparison.OrdinalIgnoreCase));

            if (persons.Any())
            {
                var firstPerson = persons.First();
                var currentActiveWindowLocation = foregroundWindow
                    .GetForegroundWindowBounds()
                    .Location;

                var objectLocation = positionCalculator
                    .RecalculateObjectPosition(currentActiveWindowLocation, 
                        Point.Round(firstPerson.Location), 
                        Size.Round(firstPerson.Size));

                // mouseControl.MoveMouseTo(objectLocation);

                Console.WriteLine($"One found at " +
                    $"{objectLocation} " +
                    $"located on desktop.");


            }
        }
    }
}
