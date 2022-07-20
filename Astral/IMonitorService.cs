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
        event EventHandler<Bitmap>? ScreenshotRendered;

        /// <summary>
        /// Event whenever we're about to screenshot.
        /// </summary>
        event EventHandler? ScreenshotStarting;

        void Stop();

        Task StartAsync();
    }
}
