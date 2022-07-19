﻿using Astral.Monitor;
using FastYolo;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astral.Detection
{
    public class FastYolo : IService, IDetectorService
    {
        private readonly IMonitorService screenGrab;
        private readonly YoloWrapper yoloWrapper;
        private readonly System.Drawing.ImageConverter converter = new();

        public FastYolo(IMonitorService screenGrab)
        {
            Console.WriteLine("FastYolo Initialized...");
            this.screenGrab = screenGrab;

            yoloWrapper = new YoloWrapper(
                "./Dependencies/FastYolo/yolov3-tiny.cfg",
                "./Dependencies/FastYolo/yolov3-tiny.weights",
                "./Dependencies/FastYolo/coco.names");

            screenGrab.Screenshot += ScreenshotReceived;


        }

        private void ScreenshotReceived(object? sender, Bitmap screenshot)
        {
            // Console.WriteLine($"Bitmap size : {e.screenshot.Size}");
            // Console.WriteLine($"Starting point : {e.start}");

            var imageBytes = (byte[])converter.ConvertTo(screenshot, typeof(byte[]))!;
            PredictionReceived?.Invoke(this,
                yoloWrapper.Detect(imageBytes).Select(x =>
                    new Models.PredictionResult
                    (
                        x.Type!, (float)x.Confidence, new Point(x.X, x.Y),
                            new Size(x.Width, x.Height), -1
                    )
                )
           );
        }

        public event EventHandler<IEnumerable<Models.PredictionResult>>? PredictionReceived;
    }
}
