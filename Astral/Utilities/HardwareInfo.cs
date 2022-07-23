using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astral.Utilities
{
    public class HardwareInfo : IUtility
    {
        private readonly Microsoft.VisualBasic.Devices.ComputerInfo computerInfo =
            new Microsoft.VisualBasic.Devices.ComputerInfo();

        public ulong GetTotalMemoryInBytes() =>
             computerInfo.TotalPhysicalMemory;

        public ulong GetMemoryLeftInBytes() =>
             computerInfo.AvailablePhysicalMemory;

    }
}
