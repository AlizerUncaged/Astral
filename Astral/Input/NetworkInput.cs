﻿using Astral.Networking;
using Astral.Utilities;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astral.Input
{
    /// <summary>
    /// Sends the input over to the network. Preferrably Astral.Puppet.
    /// </summary>
    public class NetworkInput : IService, IPredictionConsumer
    {
        private readonly IInputImage inputImage;
        private readonly IDetectorService detectorService;
        private readonly ILogger logger;
        private readonly NetListener netListener;
        private readonly PositionCalculator positionCalculator;

        public NetworkInput(
            IInputImage inputImage,
            IDetectorService detectorService,
            ILogger logger, NetListener netListener,
            Utilities.PositionCalculator positionCalculator)
        {
            this.inputImage = inputImage;
            this.detectorService = detectorService;
            this.logger = logger;
            this.netListener = netListener;
            this.positionCalculator = positionCalculator;
            detectorService.PredictionReceived += PredictionReceived;
        }

        private void PredictionReceived(object? sender, IEnumerable<Models.PredictionResult> e)
        {
            if (sender is Networking.NetClient netClient)
            {
                netClient.SendAcknowledge();

                foreach (var result in e)
                {
                    logger.Debug($"Sending inputs bounds to {netClient.NetPeer.EndPoint}");

                    netClient.Send(new Models.NetworkObjectBounds(
                            result.Location, result.Size));
                }

                return;
            }

            logger.Debug($"Sender type unkown: {sender.GetType().FullName} " +
                $"or bounds data is not on tags.");
        }
    }
}