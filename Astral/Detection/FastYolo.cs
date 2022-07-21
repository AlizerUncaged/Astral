using Astral.Models;
using Astral.Monitor;
using FastYolo;
using Serilog;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astral.Detection
{
    public class FastYolo : IService, IDetectorService, IConfiguredService<Models.ModelConfig>
    {
        private readonly YoloWrapper yoloWrapper;
        private readonly System.Drawing.ImageConverter converter = new();
        private readonly ILogger logger;

        public ModelConfig Configuration { get; }

        public FastYolo
            (IInputImage screenGrab,
            Models.ModelConfig modelConfig,
            ILogger logger)
        {
            this.Configuration = modelConfig;
            this.logger = logger;

            yoloWrapper = new YoloWrapper(Configuration.CfgFilepath!,
                Configuration.WeightsFilepath!,
                Configuration.NamesFilepath!);

            screenGrab.InputRendered += ScreenshotReceived;
        }

        private void ScreenshotReceived(object? sender, Bitmap screenshot)
        {
            var imageBytes = (byte[])converter.ConvertTo(screenshot, typeof(byte[]))!;
            PredictionReceived?.Invoke(sender,
                yoloWrapper.Detect(imageBytes).Select(x =>
                    new Models.PredictionResult
                    (
                        x.Type!, (float)x.Confidence, new Point(x.X, x.Y),
                            new Size(x.Width, x.Height), null /* FastYolo doesn't have label index. */
                    )
                    { Tag = screenshot.Tag }
                )
           );
        }

        public event EventHandler<IEnumerable<Models.PredictionResult>>? PredictionReceived;
    }
}
