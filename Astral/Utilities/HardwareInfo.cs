﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astral.Utilities
{
    public class HardwareInfo : IUtility
    {
        public ulong GetTotalMemoryInBytes() =>
             new Microsoft.VisualBasic.Devices.ComputerInfo().TotalPhysicalMemory;

        public ulong GetMemoryLeftInBytes() =>
             new Microsoft.VisualBasic.Devices.ComputerInfo().AvailablePhysicalMemory;

    }
}