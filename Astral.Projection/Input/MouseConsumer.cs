using Astral.Curses;
using Astral.Models.Packets;
using Astral.Puppet.Models;
using Astral.Puppet.Networking;
using Astral.Utilities;
using Serilog;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astral.Puppet.Input
{
    public class MouseConsumer : IService
    {
        private readonly NetListener netlistener;
        private readonly LocalMouseControl mouseControl;
        private readonly PositionCalculator positionCalculator;
        private readonly ILogger logger;
        private readonly ForegroundWindow foregroundWindow;

        public MouseConsumer(
            ForegroundWindow foregroundWindow,
            NetListener netlistener,
            LocalMouseControl mouseControl,
            PositionCalculator positionCalculator, ILogger logger)
        {
            logger.Debug($"Mouse input initialized...");

            this.foregroundWindow = foregroundWindow;
            this.netlistener = netlistener;
            this.mouseControl = mouseControl;
            this.positionCalculator = positionCalculator;
            this.logger = logger;

            netlistener.MousePositionChanged += MouseInput;
        }

        private void MouseInput(object? sender, NetworkObjectBounds e)
        {
            // logger.Debug($"Mouse input received!");

            var newLocation = positionCalculator
                .RecalculateObjectPosition(foregroundWindow
                .GetForegroundWindowBounds().Location,
                    e.Location, e.Size);

            mouseControl.MouseLocation = Point.Round(newLocation);
        }
    }
}
