using Astral.Models;
using Astral.Utilities;
using Microsoft.ML;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.ML.Transforms.Image.ImageResizingEstimator;
using YoloV4;

namespace Astral.Detection
{
    public class YoloV4 : IService, IDetectorService
    {
        private readonly ILogger logger;
        private readonly CocoClassnameProvider cocoClassnameProvider;
        private readonly MLContext mlContext = new MLContext();
        private readonly PredictionEngine<YoloV4BitmapData, YoloV4Prediction> predictionEngine;

        public event EventHandler<IEnumerable<PredictionResult>>? PredictionReceived;


        const string modelPath = @"./Dependencies/YoloV4/yolov4.onnx";

        public YoloV4(IMonitorService monitorService, ILogger logger, Utilities.CocoClassnameProvider cocoClassnameProvider)
        {
            monitorService.ScreenshotRendered += ScreenshotReceived;

            this.logger = logger;
            this.cocoClassnameProvider = cocoClassnameProvider;

            // Define scoring pipeline
            var pipeline = mlContext.Transforms.ResizeImages(inputColumnName: "bitmap", outputColumnName: "input_1:0", imageWidth: 416, imageHeight: 416, resizing: ResizingKind.IsoPad)
                .Append(mlContext.Transforms.ExtractPixels(outputColumnName: "input_1:0", scaleImage: 1f / 255f, interleavePixelColors: true))
                .Append(mlContext.Transforms.ApplyOnnxModel(
                    shapeDictionary: new Dictionary<string, int[]>()
                    {
                        { "input_1:0", new[] { 1, 416, 416, 3 } },
                        { "Identity:0", new[] { 1, 52, 52, 3, 85 } },
                        { "Identity_1:0", new[] { 1, 26, 26, 3, 85 } },
                        { "Identity_2:0", new[] { 1, 13, 13, 3, 85 } },
                    },
                    inputColumnNames: new[]
                    {
                        "input_1:0"
                    },
                    outputColumnNames: new[]
                    {
                        "Identity:0",
                        "Identity_1:0",
                        "Identity_2:0"
                    },
                    modelFile: modelPath, recursionLimit: 100));

            // Fit on empty list to obtain input data schema
            var model = pipeline.Fit(mlContext.Data.LoadFromEnumerable(new List<YoloV4BitmapData>()));

            predictionEngine = mlContext.Model.CreatePredictionEngine<YoloV4BitmapData, YoloV4Prediction>(model);

        }

        private void ScreenshotReceived(object? sender, Bitmap e)
        {
            var predict = predictionEngine.Predict(new YoloV4BitmapData() { Image = e });

            var results = predict.GetResults(cocoClassnameProvider.ClassNames, 0.5f, 0.5f);

            PredictionReceived?.Invoke(this, results.Select(x => new PredictionResult(x.Label, x.Confidence,
                    new Point((int)x.BBox[0], (int)x.BBox[1]), new Size((int)x.BBox[2] - (int)x.BBox[0], (int)x.BBox[3] - (int)x.BBox[1]),null
                )));
        }
    }
}
