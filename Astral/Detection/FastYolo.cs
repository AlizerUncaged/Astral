using Astral.Monitor;
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
        private readonly ScreenGrab screenGrab;
        private readonly YoloWrapper yoloWrapper;
        private readonly System.Drawing.ImageConverter converter = new();

        public FastYolo(Monitor.ScreenGrab screenGrab)
        {
            this.screenGrab = screenGrab;

            yoloWrapper = new YoloWrapper(
                "./Dependencies/FastYolo/yolov3-tiny.cfg",
                "./Dependencies/FastYolo/yolov3-tiny.weights", 
                "./Dependencies/FastYolo/coco.names");

            screenGrab.Screenshot += ScreenshotReceived;


        }

        private void ScreenshotReceived(object? sender, Bitmap e)
        {
            var imageBytes = (byte[])converter.ConvertTo(e, typeof(byte[]))!;
            PredictionReceived?.Invoke(this, yoloWrapper.Detect(imageBytes));
        }

        public event EventHandler<IEnumerable>? PredictionReceived;
    }
}
