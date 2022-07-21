using Autofac;
using Autofac.Features.ResolveAnything;
using Microsoft.Extensions.Configuration;
using Serilog;
using System.Reflection;

namespace Astral
{
    public class Program
    {
        private IContainer? Container { get; set; }

        public async Task StartAsync()
        {
            Build();

            await StartMainSequenceAsync();
        }

        public void Build()
        {
            var currentAssembly = Assembly.GetExecutingAssembly();

            var builder = new ContainerBuilder();

            builder.RegisterAssemblyTypes(currentAssembly)
                .AssignableTo<IConfig>()
                .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(currentAssembly)
                .AssignableTo<IUtility>()
                .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(currentAssembly)
                .AssignableTo<IService>()
                .SingleInstance();

            // Set logger.
            builder.Register<ILogger>((c, p) =>
            {
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("./appsettings.json")
                    .Build();

                return new LoggerConfiguration()
                    .ReadFrom.Configuration(configuration)
                    .CreateLogger();
            }).SingleInstance();

            // Set the current active classes here.

            builder.RegisterType<Detection.FastYolo>()
                .As<IDetectorService>()
                .SingleInstance(); // Detector class.

            builder.RegisterType<Monitor.ActiveWindowGrab>()
                .As<IInputImage>()
                .SingleInstance(); // Vision class.


            builder.RegisterType<Astral<IDetectorService, IInputImage>>()
                .As<IAstral>();

            Container = builder.Build();

        }

        public async Task StartMainSequenceAsync() =>
          await Container?.BeginLifetimeScope()
            .Resolve<IAstral>()
            .StartAsync()!;


        static void Main(string[] args) => new Program().StartAsync().GetAwaiter().GetResult();
    }
}