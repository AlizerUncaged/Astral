using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astral.Models.Configurations
{
    public class MachineInformation : IConfig
    {
        public bool IsGPUSlow { get; set; }

        public bool IsOSOld { get; set; }
    }
}
