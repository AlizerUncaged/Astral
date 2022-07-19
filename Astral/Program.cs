using Autofac;
using Autofac.Features.ResolveAnything;
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

            // Set the current active classes here.

            builder.RegisterType<Detection.FastYolo>()
                .As<IDetectorService>()
                .SingleInstance(); // Detector class.

            builder.RegisterType<Monitor.ActiveWindowGrab>()
                .As<IMonitorService>()
                .SingleInstance(); // Vision class.


            builder.RegisterType<Astral<IDetectorService, IMonitorService>>()
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