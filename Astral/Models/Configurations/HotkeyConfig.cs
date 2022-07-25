using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astral.Models.Configurations
{
    public class HotkeyConfig : IConfig
    {
        public SharpHook.Native.KeyCode PauseInputs { get; set; } =
            SharpHook.Native.KeyCode.VcLeftShift;
    }
}
