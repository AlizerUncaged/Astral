using Astral.Detection;
using Astral.Models;
using Astral.Monitor;
using Serilog;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astral.Debug
{
    public class PredictionPerformance : IService, IStoppable
    {
        private readonly IDetectorService model;
        private readonly IInputImage screenGrab;
        private readonly ILogger logger;
        private readonly ProgramStatus programStatus;

        public PredictionPerformance(IDetectorService model,
            IInputImage monitor,
            ILogger logger,
            Models.ProgramStatus programStatus)
        {
            this.model = model;
            this.screenGrab = monitor;
            this.logger = logger;
            this.programStatus = programStatus;
            monitor.InputStarting += ScreenshotStarted;
            model.PredictionReceived += Prediction;

            _ = MeasureScreenshotSpeed();
        }

        private Stopwatch? predictionCounter =
            new Stopwatch();

        private CancellationTokenSource measurementCancellationToken =
            new CancellationTokenSource();

        private long longestPrediction;

        private void ScreenshotStarted(object? sender, object s) =>
            predictionCounter?.Start();

        public async Task MeasureScreenshotSpeed()
        {
            var period = new PeriodicTimer(TimeSpan.FromSeconds(1));

            while (!programStatus.IsClosing &&
                await period.WaitForNextTickAsync(measurementCancellationToken.Token))
            {
                // Ten predictions per second is already fast.
                var color = currentPredictions >= 10 ?
                    Color.LightGreen : Color.LightCoral;

                // p/sec : predictions per second.
                logger.Information($"{currentPredictions} p/sec, " +
                    $"longest prediction: {longestPrediction}ms");

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
                longestPrediction = predictionCounter.ElapsedMilliseconds >
                    longestPrediction ? predictionCounter.ElapsedMilliseconds
                    : longestPrediction;

                predictionCounter.Reset();
            }
        }

        public void Stop()
        {
            measurementCancellationToken.Cancel();
        }
    }
}
