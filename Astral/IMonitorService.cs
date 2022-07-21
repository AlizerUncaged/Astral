using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astral
{
    public interface IInputImage
    {
        /// <summary>
        /// Event called whenever the screenshot occured.
        /// </summary>
        event EventHandler<Bitmap>? InputRendered;

        /// <summary>
        /// Event whenever we're about to screenshot.
        /// </summary>
        event EventHandler? InputStarting;

        void Stop();

        Task StartAsync();
    }
}
