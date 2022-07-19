using Astral.Monitor;
using Autofac;
using Pastel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yolov5Net.Scorer;
using Yolov5Net.Scorer.Models;

namespace Astral.Detection
{
    public class YoloV5 : IService, IDetectorService
    {
        private readonly YoloScorer<YoloCocoP5Model> scorer;
        private readonly ScreenGrab screenGrab;

        public YoloV5(Monitor.ScreenGrab screenGrab)
        {
            this.screenGrab = screenGrab;
            screenGrab.Screenshot += ScreenshotReceived;

            // Use the small YOLOv5 model.
            scorer = new YoloScorer<YoloCocoP5Model>("./Dependencies/YoloV5/yolov5s.onnx");
        }

        /// <summary>ss
        /// Get's called whenever a prediction occurs.
        /// </summary>
        public event EventHandler<IEnumerable>? PredictionReceived;

        private void ScreenshotReceived(object? sender, Bitmap e) =>
            PredictionReceived?.Invoke(this, scorer.Predict(e));
        
    }
}
