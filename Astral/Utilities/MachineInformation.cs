using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astral.Utilities
{
    public class MachineInformation :
        IService,
        IUtility
    {
        private readonly HardwareInfo hardwareInfo;
        private readonly SizeFormatProvider sizeFormatProvider;

        public MachineInformation(HardwareInfo hardwareInfo, SizeFormatProvider sizeFormatProvider)
        {
            this.hardwareInfo = hardwareInfo;
            this.sizeFormatProvider = sizeFormatProvider;
        }

        public async Task<Models.Configurations.MachineInformation> GetMachineInformation()
        {
            Models.Configurations.MachineInformation
                machineInformation = new();

            // TODO

            return machineInformation;
        }

        public async Task<bool> IsAvailableRamTooLow() =>
            await Task.Run(() => { return hardwareInfo.GetMemoryLeftInBytes() <
                sizeFormatProvider.GigabytesToBytes(1); });

    }
}
