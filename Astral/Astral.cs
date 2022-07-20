using Astral.Debug;
using Astral.Monitor;
using Astral.Utilities;
using Pastel;
using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astral.Detection;
using System.Diagnostics;

namespace Astral
{
    public interface IAstral
    {
        Task StartAsync();
    }

    public class Astral<Detector, Vision> : IAstral
        where Detector : IDetectorService
        where Vision : IMonitorService
    {
        private readonly Vision vision;
        private readonly HardwareInfo hardwareInfo;
        private readonly Detector detector;

        public Astral(
            Vision screenGrab,
            Utilities.HardwareInfo hardwareInfo,
            Detector model, // Or Detection.FastYolo, both are pretty much the same.
            Debug.PredictionPerformance predictionPerformance,
            Debug.PredictionEnumerizer predictionEnumerizer)
        {
            this.vision = screenGrab;
            this.hardwareInfo = hardwareInfo;
            this.detector = model;

            Console.CancelKeyPress += Closing;
        }

        private void Closing(object? sender, ConsoleCancelEventArgs e)
        {
            Console.WriteLine($"Exiting...".Pastel(Color.DarkGray));
            vision.Stop();

            // Commit sepuku.
            Process.GetCurrentProcess().Kill();
        }

        public async Task StartAsync()
        {
            Console.Title = "Astral";

            if (hardwareInfo.GetMemoryLeftInBytes() < 1073741824)
                Console.WriteLine($"! {"Available ram too low".Pastel(Color.LightCoral)}, " +
                    $"below 1GB, Astral might crash.");

            Console.WriteLine($"Detector : {$"{detector}".Pastel(Color.LightCyan)}");
            Console.WriteLine($"Vision : {$"{vision}".Pastel(Color.LightCyan)}");

            Console.WriteLine($"{$"{DateTime.Now}".Pastel(Color.DarkGray)} " +
                $"{"Vision started".Pastel(Color.LightGreen)}...");

            await vision.StartAsync();
        }
    }
}
