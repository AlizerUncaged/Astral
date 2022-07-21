using Astral.Debug;
using Astral.Monitor;
using Astral.Utilities;
using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astral.Detection;
using System.Diagnostics;
using Serilog;

namespace Astral
{
    public interface IAstral
    {
        Task StartAsync();
    }

    public class Astral<Detector, Vision> : IAstral
        where Detector : IDetectorService
        where Vision : IInputImage
    {
        private readonly Vision vision;
        private readonly HardwareInfo hardwareInfo;
        private readonly Detector detector;
        private readonly ILogger logger;

        public Astral(
            Vision screenGrab,
            Utilities.HardwareInfo hardwareInfo,
            Detector model, // Or Detection.FastYolo, both are pretty much the same.
            Debug.PredictionPerformance predictionPerformance,
            Debug.PredictionEnumerizer predictionEnumerizer, ILogger logger)
        {
            this.vision = screenGrab;
            this.hardwareInfo = hardwareInfo;
            this.detector = model;
            this.logger = logger;
            Console.CancelKeyPress += Closing;
        }

        private void Closing(object? sender, ConsoleCancelEventArgs e)
        {
            logger.Information("Exiting...");
            vision.Stop();

            // Commit sepuku.
            Process.GetCurrentProcess().Kill();
        }

        public async Task StartAsync()
        {
            Console.Title = "Astral";

            if (hardwareInfo.GetMemoryLeftInBytes() < 1073741824)
                logger.Warning($"Available ram too low, below 1GB, Astral might crash.");

            logger.Information($"Detector : {detector}");
            logger.Information($"Vision : {vision}");
            logger.Information($"Vision started...");

            await vision.StartAsync();
        }
    }
}
