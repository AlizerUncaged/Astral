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

        const float minimumConfidence = 0.4f;
        public PredictionEnumerizer(Detection.YoloV5 model)
        {
            this.model = model;
            model.PredictionReceived += PredictionReceived;
        }

        private void PredictionReceived(object? sender, IEnumerable e)
        {
            if (e is IEnumerable<YoloPrediction> yolov5)
            {
                var highConfidenceObjects = yolov5.Where(x => x.Score > minimumConfidence);

                if (highConfidenceObjects.Count() > 0)
                    Console.WriteLine($"{$"{highConfidenceObjects.Count()}".Pastel(Color.LightBlue)} Objects: " +
                        $"{$"{string.Join(", ", highConfidenceObjects.Select(x => x.Label.Name).Distinct())}".Pastel(Color.LightGreen)}".Pastel(Color.DarkGray));
                else Console.WriteLine("No objects recognized.".Pastel(Color.LightGray));
            }
            else if (e is IEnumerable<FastYolo.Model.YoloItem> fastYoloResult)
            {
                var highConfidenceObjects = fastYoloResult.Where(x => x.Confidence > minimumConfidence);

                if (highConfidenceObjects.Count() > 0)
                    Console.WriteLine($"{$"{highConfidenceObjects.Count()}".Pastel(Color.LightBlue)} Objects: " +
                        $"{$"{string.Join(", ", highConfidenceObjects.Select(x => x.Type).Distinct())}".Pastel(Color.LightGreen)}".Pastel(Color.DarkGray));
                else Console.WriteLine("No objects recognized.".Pastel(Color.LightGray));

            }
        }
    }
}
