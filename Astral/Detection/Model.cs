using Astral.Monitor;
using Autofac;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yolov5Net.Scorer;
using Yolov5Net.Scorer.Models;

namespace Astral.Detection
{
    public class Model : IService
    {
        private readonly YoloScorer<YoloCocoP5Model> scorer;
        private readonly ScreenGrab screenGrab;

        public Model(ILifetimeScope scope, Monitor.ScreenGrab screenGrab)
        {
            this.screenGrab = screenGrab;
            screenGrab.Screenshot += ScreenshotReceived;

            scorer = new YoloScorer<YoloCocoP5Model>("./yolov5s.onnx");
        }

        /// <summary>
        /// Get's called whenever a prediction occurs.
        /// </summary>
        public event EventHandler<Bitmap> Prediction;

        private void ScreenshotReceived(object? sender, Bitmap e)
        {
            Bitmap clone = (Bitmap)e.Clone();

            List<YoloPrediction> predictions = scorer.Predict(clone);

            using var graphics = Graphics.FromImage(clone);

            foreach (var prediction in predictions) // iterate predictions to draw results
            {
                double score = Math.Round(prediction.Score, 2);

                if (score >= 0.4)
                {
                    graphics.DrawRectangles(new Pen(prediction.Label.Color, 2),
                        new[] { prediction.Rectangle });

                    var (x, y) = (prediction.Rectangle.X - 3, prediction.Rectangle.Y - 23);

                    graphics.DrawString($"{prediction.Label.Name} ({score})",
                        new Font("Arial", 18, GraphicsUnit.Pixel), new SolidBrush(prediction.Label.Color),
                        new PointF(x, y));
                }
            }

            Prediction?.Invoke(this, clone);
        }
    }
}
