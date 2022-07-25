using Astral.Models;
using Autofac;
using Autofac.Features.ResolveAnything;
using Microsoft.Extensions.Configuration;
using Serilog;
using System.Diagnostics;
using System.Reflection;

namespace Astral
{
    public class Program
    {
        private IContainer? container;
        private ILifetimeScope? lifetimeScope;

        public async Task StartAsync()
        {
            Build();

            await StartMainSequenceAsync();
        }

        public void Build()
        {
            var modelConfig = new Models.Configurations.ModelConfig();
            var screenConfig = new Models.Configurations.ScreenConfig();
            var hotkeyConfig = new Models.Configurations.HotkeyConfig();
            var networkConfig = new Models.Configurations.NetworkConfig();
            var predictionConfig = new Models.Configurations.PredictionConfig();

            //// Coco dataset.
            //modelConfig = new Models.Configurations.ModelConfig
            //{
            //    CfgFilepath = "./Dependencies/FastYolo/yolov3-tiny.cfg",
            //    NamesFilepath = "./Dependencies/FastYolo/coco.names",
            //    WeightsFilepath = "./Dependencies/FastYolo/yolov3-tiny.weights"
            //};

            modelConfig = new Models.Configurations.ModelConfig
            {
                CfgFilepath = "./Dependencies/YoloV4/CSGO/csgo.cfg",
                NamesFilepath = "./Dependencies/YoloV4/CSGO/csgo.names",
                WeightsFilepath = "./Dependencies/YoloV4/CSGO/csgo.weights"
            };

            container = new AstralProgramBuilder<
                Detection.FastYolo,
                Monitor.ActiveWindowGrab,
                Input.LocalInput>()
                .Build(
                        new IConfig[] { modelConfig, 
                            screenConfig,
                            networkConfig,
                            predictionConfig,
                            hotkeyConfig }
                      );
        }

        public async Task StartMainSequenceAsync()
        {
            Console.CancelKeyPress += Closing;

            using (lifetimeScope = container?.BeginLifetimeScope())
                await lifetimeScope?.Resolve<IAstral>().StartAsync()!;
        }

        private void Closing(object? sender, ConsoleCancelEventArgs e)
        {
            lifetimeScope?.Resolve<IAstral>().Stop();

            // Required so CUDA can properly end.
            // Another Ctrl + C is required.
            e.Cancel = true;
        }

        static void Main(string[] args) =>
            new Program().StartAsync().GetAwaiter().GetResult();
    }
}