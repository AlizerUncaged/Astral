using Astral.Networking;
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
    /// <typeparam name="ImageCompressor">The image compressor class to use.</typeparam>
    /// <typeparam name="PredictionConsumer">The class that makes do with the prediction result.</typeparam>
    public class AstralProgramBuilder<Detector, InputFrom, PredictionConsumer>
        where Detector : IDetectorService
        where InputFrom : IInputImage
        where PredictionConsumer : IPredictionConsumer
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
                .SingleInstance();

            builder.RegisterAssemblyTypes(currentAssembly)
                .AssignableTo<IService>()
                .SingleInstance();



            // Set the current active classes here.
            builder.RegisterType<Detector>()
                .AsImplementedInterfaces()
                .AsSelf()
                .SingleInstance(); // Detector class.

            builder.RegisterType<InputFrom>()
                .AsImplementedInterfaces()
                .AsSelf()
                .SingleInstance(); // Vision class.

            builder.RegisterType<PredictionConsumer>()
                .AsImplementedInterfaces()
                .AsSelf()
                .SingleInstance(); // Prediction handler class.

            builder.RegisterType<NetListener>()
                .AsImplementedInterfaces()
                .AsSelf()
                .SingleInstance(); // Network handler class.

            builder.RegisterType<Astral<IDetectorService, IInputImage>>()
                .As<IAstral>()
                .SingleInstance(); // Add the main class.

            RegisterLogger(builder);

            return builder.Build();
        }

        public void RegisterLogger(ContainerBuilder containerBuilder)
        {
            var appConfiguration = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("./appsettings.json").Build();

            // Set logger.
            Log.Logger = new LoggerConfiguration()
                    .ReadFrom.Configuration(appConfiguration)
                    .CreateLogger();

            containerBuilder.RegisterLogger();
        }
    }
}
