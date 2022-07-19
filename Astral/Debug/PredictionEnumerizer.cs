using Astral.Curses;
using Astral.Detection;
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
        private readonly YoloV5 model;
        private readonly MouseControl mouseControl;

        const float minimumConfidence = 0.4f;
        public PredictionEnumerizer(Detection.YoloV5 model, Curses.MouseControl mouseControl)
        {
            this.model = model;
            this.mouseControl = mouseControl;
            model.PredictionReceived += PredictionReceived;
        }

        private void PredictionReceived(object? sender, IEnumerable<Models.PredictionResult> e)
        {
            var highConfidenceObjects = e.Where(x => x.Score > minimumConfidence);

            if (!highConfidenceObjects.Any())
            {
                Console.WriteLine("No objects recognized.".Pastel(Color.LightGray));
                return;
            }

            Console.WriteLine($"{$"{highConfidenceObjects.Count()}".Pastel(Color.LightBlue)} Objects: " +
                $"{$"{string.Join(", ", highConfidenceObjects.Select(x => x.Label).Distinct())}".Pastel(Color.LightGreen)}".Pastel(Color.DarkGray));
         
            var persons = highConfidenceObjects.Where(x => x.LabelIndex == 1); // 1 = Person

            if (persons.Any())
            {
                var firstPerson = persons.First();
                mouseControl.MoveMouseTo(firstPerson.Location);
                // Console.WriteLine($"Person found at {firstPerson.Location}");
                // Console.WriteLine($"Mouse position at {Cursor.Position}");
            }
        }
    }
}
