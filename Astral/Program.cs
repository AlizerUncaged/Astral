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
            var networkConfig = new Models.Configurations.NetworkConfig();
            var predictionConfig = new Models.Configurations.PredictionConfig();

            container = new AstralProgramBuilder<
                Detection.FastYolo,
                Monitor.ImageFromPeer,
                Input.NetworkInput>()
                .Build(
                        new IConfig[] { modelConfig, screenConfig, networkConfig, predictionConfig }
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

        static void Main(string[] args)=>
            new Program().StartAsync().GetAwaiter().GetResult(); 
    }
}