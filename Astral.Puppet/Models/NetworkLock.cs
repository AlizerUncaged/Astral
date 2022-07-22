using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astral.Puppet.Models
{
    public class NetworkLock
    {
        public const int MaxSimultaneousScreenshotSend = 2;

        /// <summary>
        /// Max waiting time for server to send acknowledgement.
        /// </summary>
        public int MaxWaitTimeout => 1000;

        /// <summary>
        /// Network lock, wait for server output before sending any
        /// screenshot.
        /// </summary>
        public SemaphoreSlim Lock { get; set; } = new SemaphoreSlim(MaxSimultaneousScreenshotSend,
            MaxSimultaneousScreenshotSend);
    }
}
