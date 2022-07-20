using Astral.Detection;
using Astral.Monitor;
using Pastel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astral.Debug
{
    public class PredictionPerformance : IService
    {
        private readonly IDetectorService model;
        private readonly IMonitorService screenGrab;

        public PredictionPerformance(IDetectorService model, IMonitorService monitor)
        {
            this.model = model;
            this.screenGrab = monitor;

            monitor.ScreenshotStarting += ScreenshotStarted;
            model.PredictionReceived += Prediction;


            _ = MeasureScreenshotSpeed();
        }


        private Stopwatch? predictionCounter;
        private double longestPrediction;

        private void ScreenshotStarted(object? sender, object s) =>
            predictionCounter = Stopwatch.StartNew();


        public async Task MeasureScreenshotSpeed()
        {
            var period = new PeriodicTimer(TimeSpan.FromSeconds(1));

            while (await period.WaitForNextTickAsync())
            {
                // Ten predictions per second is already fast.
                var color = currentPredictions >= 10 ?
                    Color.LightGreen : Color.LightCoral;

                Console.WriteLine($"{$"{currentPredictions}".Pastel(color)} " +
                    $"{$"p/sec, longest prediction: ".Pastel(Color.DarkGray)}" +
                    $"{$"{Math.Ceiling(longestPrediction)}".Pastel(color)}{$"ms".Pastel(Color.DarkGray)}");

                longestPrediction = 0;
                currentPredictions = 0;
            }
        }

        private int currentPredictions = 0;
        private void Prediction(object? sender, IEnumerable e)
        {
            currentPredictions++;

            if (predictionCounter is { })
            {
                longestPrediction = predictionCounter.ElapsedMilliseconds > longestPrediction ?
                    predictionCounter.ElapsedMilliseconds : longestPrediction;
                predictionCounter = null;
            }
        }
    }
}
