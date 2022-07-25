using Astral.Models;
using Astral.Models.Configurations;
using Astral.Monitor;
using Astral.Utilities;
using FastYolo;
using Serilog;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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
        private readonly ConverterUtility converterUtility;
        private readonly AstralStatus status;
        private readonly ILogger logger;

        public ModelConfig Configuration { get; }

        public FastYolo
            (IInputImage screenGrab, Utilities.ConverterUtility converterUtility,
            ModelConfig modelConfig, Models.AstralStatus status,
            ILogger logger)
        {
            this.converterUtility = converterUtility;
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

            byte[] imageBytes = converterUtility.ImageToBytes(screenshot);

            PredictionReceived?.Invoke(sender,
                yoloWrapper.Detect(imageBytes).Select(result =>
                    new PredictionResult
                    (
                        result.Type!, (float)result.Confidence, new Point(result.X, result.Y),
                            new Size(result.Width, result.Height),
                            null /* FastYolo doesn't have label index. */
                    )
                    {
                        ObjectId = result.TrackId
                    }
                )
           );
        }

        public void Stop()
        {

        }

        public event EventHandler<IEnumerable<PredictionResult>>? PredictionReceived;
    }
}
