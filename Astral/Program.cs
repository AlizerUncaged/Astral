using static Tensorflow.Binding;
using static Tensorflow.KerasApi;
using Tensorflow;
using Tensorflow.NumPy;

using static SharpCV.Binding;

namespace Astral
{
    public class Program
    {
        private ScreenGrab screenGrab = new ScreenGrab(2, 15, Screen.PrimaryScreen);

        public async Task StartAsync()
        {
            Console.WriteLine($"Program started {DateTime.Now}");

            screenGrab.Screenshot += OnScreenshot;

            Console.WriteLine($"Vision started {DateTime.Now}");

            var oneSecondTimer = new PeriodicTimer(TimeSpan.FromSeconds(1));
            _ = Task.Run(async () =>
            {
                while (await oneSecondTimer.WaitForNextTickAsync())
                {
                    Console.WriteLine($"{DateTime.Now} - {screenshots} fps");
                    screenshots = 0;
                }
            });

            await screenGrab.StartPeriodicScreenshotAsync(); // Run async.
        }

        private int screenshots = 0;

        private void OnScreenshot(object? sender, SharpCV.Mat e)
        {
            screenshots++;
        }

        static void Main(string[] args) => new Program().StartAsync().GetAwaiter().GetResult();

    }
}