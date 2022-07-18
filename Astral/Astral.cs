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

namespace Astral
{
    public class Astral : IService
    {
        private readonly ScreenGrab screenMonitor;
        private readonly HardwareInfo hardwareInfo;
        private readonly Model detector;

        public Astral(Monitor.ScreenGrab screenGrab, Utilities.HardwareInfo hardwareInfo,
            Detection.Model model, Debug.DebugWindow debugWindow)
        {
            this.screenMonitor = screenGrab;
            this.hardwareInfo = hardwareInfo;
            this.detector = model;
        }

        public bool AutoStart => true;

        public async Task StartAsync()
        {
            if (hardwareInfo.GetMemoryLeftInBytes() < 2147483648)
                Console.WriteLine($"! {"Available ram too low".Pastel(Color.LightCoral)}, " +
                    $"below 2GB, Astral might crash.");

            Console.WriteLine($"{$"{DateTime.Now}".Pastel(Color.DarkGray)} " +
                $"{"Vision started".Pastel(Color.LightGreen)}...");

            await screenMonitor.StartPeriodicScreenshotAsync();
        }
    }
}
