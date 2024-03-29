﻿using Astral.Models;
using Astral.Monitor;
using Autofac;
using Microsoft.ML.OnnxRuntime;
using Serilog;
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
    public class YoloV5 : IDetectorService, IStoppable
    {
        private readonly YoloScorer<YoloCocoP5Model> scorer;
        private readonly ILogger ilogger;
        private readonly AstralStatus status;

        public YoloV5(IInputImage monitorService, ILogger ilogger, Models.AstralStatus astralStatus)
        {
            monitorService.InputRendered += ScreenshotReceived;

            // Use the small YOLOv5 model.
            scorer = new YoloScorer<YoloCocoP5Model>("./Dependencies/YoloV5/yolov5s.onnx",
                SessionOptions.MakeSessionOptionWithCudaProvider());
            this.ilogger = ilogger;
            this.status = astralStatus;
        }

        /// <summary>ss
        /// Get's called whenever a prediction occurs.
        /// </summary>
        public event EventHandler<IEnumerable<PredictionResult>>? PredictionReceived;

        public void Stop()
        {
            scorer.Dispose();
        }

        private void ScreenshotReceived(object? sender, Bitmap screenshot)
        {
            if (status.IsClosing ||
                status.IsPredictionPaused)
                return;

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
