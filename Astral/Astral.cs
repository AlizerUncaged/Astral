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
using Astral.Models;

namespace Astral
{
    public interface IAstral
    {
        Task StartAsync();

        void Stop();
    }

    public class Astral<Detector, Vision> : IAstral
        where Detector : IDetectorService
        where Vision : IInputImage
    {
        private readonly ILifetimeScope scope;
        private readonly IInputImage vision;
        private readonly IDetectorService detector;
        private readonly ILogger logger;

        private readonly HardwareInfo hardwareInfo;
        private readonly MachineInformation machineInformation;
        private readonly AstralStatus programStatus;

        public Astral(
            ILifetimeScope scope,
            Vision screenGrab,
            IPredictionConsumer inputConsumer,
            ILogger logger,
            Detector model, // Or Detection.FastYolo, both are pretty much the same.
            HardwareInfo hardwareInfo,
            PredictionPerformance predictionPerformance,
            PredictionEnumerator predictionEnumerator,
            MachineInformation machineInformation,
            AstralStatus programStatus)
        {
            this.scope = scope;
            this.vision = screenGrab;
            this.hardwareInfo = hardwareInfo;
            this.machineInformation = machineInformation;
            this.programStatus = programStatus;
            this.detector = model;
            this.logger = logger;
        }


        public void Stop()
        {
            logger.Information("Exiting...");

            programStatus.CloseProgram();

            // Dispose the disposables.
            var stoppableRegistrations = scope.ComponentRegistry.Registrations
                    .Where(x => typeof(IStoppable).IsAssignableFrom(x.Activator.LimitType));

            var stoppableLimitTypes = stoppableRegistrations
                .Select(x => x.Activator.LimitType).Where(x => scope.IsRegistered(x));

            logger.Debug($"Stoppable types : {string.Join(", ", stoppableLimitTypes.Select(x => x.FullName))}");

            foreach (var s in stoppableLimitTypes)
            {
                logger.Debug($"Stopping {s.FullName}");

                var resolved = scope.Resolve(s) as IStoppable;
                resolved?.Stop();
            }
        }

        public async Task StartAsync()
        {
            Console.Title = "Astral";

            if (await machineInformation.IsAvailableRamTooLow())
                logger.Warning($"Available ram too low, below 1GB, Astral might crash.");

            logger.Information($"Detector : {detector}");
            logger.Information($"Vision : {vision}");
            logger.Information($"Vision started...");

            await vision.StartAsync();

            logger.Debug($"Vision ended...");
        }
    }
}
