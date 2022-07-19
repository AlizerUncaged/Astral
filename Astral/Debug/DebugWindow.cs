using Astral.Detection;
using Astral.Monitor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astral.Debug
{
    public class DebugWindow : IService
    {
        private readonly ScreenGrab screenGrab;
        private readonly ScreenConfig configuration;
        private readonly Model model;
        private DebugForm debugForm;

        public DebugWindow(Monitor.ScreenGrab screenGrab, ScreenConfig configuration, Detection.Model model)
        {
            this.screenGrab = screenGrab;
            this.configuration = configuration;
            this.model = model;

            // TODO

            _ = Task.Run(() =>
            {
                debugForm = new DebugForm();
                debugForm.ShowDialog();
            });
        }

        private void ScreenshotReceived(object? sender, Bitmap e)
        {
            if (debugForm is { } && !debugForm.IsDisposed)
                debugForm.SetImage(e);
        }
    }
}
