using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astral.Models
{
    public class MachineInformation : IUtility
    {
        public bool IsGpuSlow { get; set; }

        public bool IsOsOld { get; set; }
    }
}
