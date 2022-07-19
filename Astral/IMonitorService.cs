using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astral
{
    public interface IMonitorService
    {
        /// <summary>
        /// Event called whenever the screenshot occured.
        /// </summary>
        event EventHandler<Bitmap>? Screenshot;

        /// <summary>
        /// Event whenever we're about to screenshot.
        /// </summary>
        event EventHandler? ScreenshotStarted;

        void Stop();

        Task StartAsync();
    }
}
