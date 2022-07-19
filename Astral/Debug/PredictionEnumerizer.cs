using Astral.Detection;
using Pastel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astral.Debug
{
    public class PredictionEnumerizer : IService
    {
        private readonly Model model;

        const float minimumConfidence = 0.4f;
        public PredictionEnumerizer(Detection.Model model)
        {
            this.model = model;
            model.PredictionReceived += PredictionReceived;
        }

        private void PredictionReceived(object? sender, IEnumerable<Yolov5Net.Scorer.YoloPrediction> e)
        {
            var highConfidenceObjects = e.Where(x => x.Score > minimumConfidence);

            if (highConfidenceObjects.Count() > 0)
                Console.WriteLine($"{$"{highConfidenceObjects.Count()}".Pastel(Color.LightBlue)} Objects: " +
                    $"{$"{string.Join(", ", highConfidenceObjects.Select(x => x.Label.Name).Distinct())}".Pastel(Color.LightGreen)}".Pastel(Color.DarkGray));
            else Console.WriteLine("No objects recognized.".Pastel(Color.LightGray));
        }
    }
}
