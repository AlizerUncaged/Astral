using Astral.Models;
using Astral.Models.Configurations;
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
    /*
     * Speed on an MX330 Laptop:
     * Downscale at 0.5 or 50% below 10 p/sec
     * No downscale below 5 p/sec
     */
    public class FastYolo : IDetectorService, IConfiguredService<ModelConfig>, IStoppable
    {
        private readonly YoloWrapper yoloWrapper;
        private readonly System.Drawing.ImageConverter converter = new();
        private readonly AstralStatus status;
        private readonly ILogger logger;

        public ModelConfig Configuration { get; }

        public FastYolo
            (IInputImage screenGrab,
            ModelConfig modelConfig, Models.AstralStatus status,
            ILogger logger)
        {
            this.Configuration = modelConfig;
            this.status = status;
            this.logger = logger;

            yoloWrapper = new YoloWrapper(Configuration.CfgFilepath!,
                Configuration.WeightsFilepath!,
                Configuration.NamesFilepath!);

            screenGrab.InputRendered += ScreenshotReceived;
        }

        private void ScreenshotReceived(object? sender, Bitmap screenshot)
        {
            if (status.IsClosing)
                return;

            logger.Debug($"Received image of size {screenshot.Size}");
            var imageBytes = (byte[])converter.ConvertTo(screenshot, typeof(byte[]))!;
            PredictionReceived?.Invoke(sender,
                yoloWrapper.Detect(imageBytes).Select(x =>
                    new Models.PredictionResult
                    (
                        x.Type!, (float)x.Confidence, new Point(x.X, x.Y),
                            new Size(x.Width, x.Height), null /* FastYolo doesn't have label index. */
                    )
                )
           );
        }

        public void Stop()
        {

        }

        public event EventHandler<IEnumerable<PredictionResult>>? PredictionReceived;
    }
}
