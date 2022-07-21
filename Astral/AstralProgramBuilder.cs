using Autofac;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Astral
{
    /// <summary>
    /// Create a new Astral instance builder.
    /// </summary>
    /// <typeparam name="Detector">The detector to use.</typeparam>
    /// <typeparam name="Input">The input class to use.</typeparam>
    public class AstralProgramBuilder<Detector, Input, ImageCompressor>
        where Detector : IDetectorService
        where Input : IInputImage
        where ImageCompressor : IImageCompressor
    {
        public IContainer Build(IEnumerable<IConfig>? configurations = null)
        {
            var currentAssembly = Assembly.GetExecutingAssembly();

            var builder = new ContainerBuilder();

            // Register configurations.
            if (configurations is { } && configurations.Any())
                configurations.Select(x => builder
                                    .RegisterInstance(x)
                                    .AsSelf()
                                    .SingleInstance()).ToList();

            builder.RegisterAssemblyTypes(currentAssembly)
                .AssignableTo<IUtility>()
                .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(currentAssembly)
                .AssignableTo<IService>()
                .SingleInstance();

            // Set logger.
            builder.Register<Serilog.ILogger>((c, p) =>
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
            builder.RegisterType<Detector>()
                .As<IDetectorService>()
                .SingleInstance(); // Detector class.

            builder.RegisterType<ImageCompressor>()
                .As<IImageCompressor>()
                .SingleInstance(); // Detector class.

            builder.RegisterType<Input>()
                .As<IInputImage>()
                .SingleInstance(); // Vision class.


            builder.RegisterType<Astral<IDetectorService, IInputImage>>()
                .As<IAstral>();

            return builder.Build();
        }
    }
}
