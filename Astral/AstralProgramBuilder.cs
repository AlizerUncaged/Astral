using Autofac;
using AutofacSerilogIntegration;
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
    /// <typeparam name="InputFrom">The input class to use.</typeparam>
    public class AstralProgramBuilder<Detector, InputFrom, ImageCompressor, InputConsumer>
        where Detector : IDetectorService
        where InputFrom : IInputImage
        where ImageCompressor : IImageCompressor
        where InputConsumer : IInputConsumer
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


            var appConfiguration = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("./appsettings.json")
                        .Build();

            // Set logger.
            Log.Logger = new LoggerConfiguration()
                    .ReadFrom.Configuration(appConfiguration)
                    .CreateLogger();

            // Set the current active classes here.
            builder.RegisterType<Detector>()
                .As<IDetectorService>()
                .SingleInstance(); // Detector class.

            builder.RegisterType<ImageCompressor>()
                .As<IImageCompressor>()
                .SingleInstance(); // Detector class.

            builder.RegisterType<InputFrom>()
                .As<IInputImage>()
                .SingleInstance(); // Vision class.

            builder.RegisterType<InputConsumer>()
                .As<IInputConsumer>()
                .SingleInstance(); // Input sender class.

            builder.RegisterType<Astral<IDetectorService, IInputImage>>()
                .As<IAstral>();

            builder.RegisterLogger();

            return builder.Build();
        }
    }
}
