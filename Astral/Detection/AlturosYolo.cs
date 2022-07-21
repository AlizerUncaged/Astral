using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astral.Detection
{
    public class AlturosYolo : IService, IDetectorService
    {
        public event EventHandler<IEnumerable<Models.PredictionResult>>? PredictionReceived;

        private readonly Alturos.Yolo.YoloWrapper yoloWrapper;
        private readonly System.Drawing.ImageConverter converter = new();
        public AlturosYolo(IInputImage monitorService)
        {
            yoloWrapper = new Alturos.Yolo.YoloWrapper("./Dependencies/YoloFastest/yolo-fastest.cfg",
                "./Dependencies/YoloFastest/yolo-fastest.weights", "./Dependencies/FastYolo/coco.names",
                new Alturos.Yolo.GpuConfig { GpuIndex = 0 });

            monitorService.InputRendered += ScreenshotRendered;
        }

        private void ScreenshotRendered(object? sender, Bitmap e)
        {
            var imageBytes = (byte[])converter.ConvertTo(e, typeof(byte[]))!;
            var result = yoloWrapper.Detect(imageBytes);

            PredictionReceived?.Invoke(this, result.Select(x =>
                 new Models.PredictionResult
                 (
                     x.Type!, (float)x.Confidence, new Point(x.X, x.Y),
                         new Size(x.Width, x.Height), null /* FastYolo doesn't have label index. */
                 )
             )
        );
        }
    }
}
