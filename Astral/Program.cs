using Autofac;
using Autofac.Features.ResolveAnything;
using Microsoft.Extensions.Configuration;
using Serilog;
using System.Reflection;

namespace Astral
{
    public class Program
    {
        private IContainer? container;

        public async Task StartAsync()
        {
            Build();

            await StartMainSequenceAsync();
        }

        public void Build()
        {
            var modelConfig = new Models.ModelConfig();
            var screenConfig = new Models.ScreenConfig();
            var networkConfig = new Models.NetworkConfig();

            container = new AstralProgramBuilder<
                Detection.FastYolo,
                Monitor.ActiveWindowGrab,
                Utilities.DefaultImageCompressor,
                Input.LocalInput>().Build(
                    new IConfig[] { modelConfig, screenConfig, networkConfig }
                );
        }

        public async Task StartMainSequenceAsync() =>
          await container?.BeginLifetimeScope()
            .Resolve<IAstral>()
            .StartAsync()!;


        static void Main(string[] args) => new Program().StartAsync().GetAwaiter().GetResult();
    }
}