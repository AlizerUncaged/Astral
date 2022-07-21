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
            container = new AstralProgramBuilder<
                Detection.FastYolo,
                Monitor.ActiveWindowGrab,
                Utilities.DefaultImageCompressor>().Build(
                    new IConfig[] { new Models.ModelConfig(), new Models.ScreenConfig() }
                );
        }

        public async Task StartMainSequenceAsync() =>
          await container?.BeginLifetimeScope()
            .Resolve<IAstral>()
            .StartAsync()!;


        static void Main(string[] args) => new Program().StartAsync().GetAwaiter().GetResult();
    }
}