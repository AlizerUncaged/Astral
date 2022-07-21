﻿using Astral.Monitor;
using Autofac;
using Microsoft.ML.OnnxRuntime;
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

        public YoloV5(IInputImage monitorService)
        {
            monitorService.InputRendered += ScreenshotReceived;

            // Use the small YOLOv5 model.
            scorer = new YoloScorer<YoloCocoP5Model>("./Dependencies/YoloV5/yolov5s.onnx",
                SessionOptions.MakeSessionOptionWithCudaProvider());
        }

        /// <summary>ss
        /// Get's called whenever a prediction occurs.
        /// </summary>
        public event EventHandler<IEnumerable<Models.PredictionResult>>? PredictionReceived;

        private void ScreenshotReceived(object? sender, Bitmap screenshot)
        {
            var prediction = scorer.Predict(screenshot);
            PredictionReceived?.Invoke(this,
                    prediction.Select(x => new Models.PredictionResult(
                            x.Label.Name,
                            x.Score,
                            Point.Round(x.Rectangle.Location),
                            Size.Round(x.Rectangle.Size),
                            x.Label.Id
                        )
                    )
                );
        }

    }
}
