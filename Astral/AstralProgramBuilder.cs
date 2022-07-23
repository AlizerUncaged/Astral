using Astral.Models;
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
    /// <typeparam name="PredictionConsumer">The class that makes do with the prediction result.</typeparam>
    public class AstralProgramBuilder<Detector, InputFrom, PredictionConsumer>
        where Detector : IDetectorService
        where InputFrom : IInputImage
        where PredictionConsumer : IPredictionConsumer
    {
        private ContainerBuilder builder = new ContainerBuilder();
        private Assembly currentAssembly = Assembly.GetExecutingAssembly();

        public IContainer Build(IEnumerable<IConfig>? configurations = null)
        {
            // Register configurations.
            if (configurations is { })
                configurations.Select(x => builder.RegisterInstance(x).AsSelf()
                                    .SingleInstance()).ToList();

            RegisterAll<IUtility>();

            RegisterAll<IService>();

            RegisterActiveClasses();

            RegisterLogger();

            // We should write that if at least one class has the RequiresNetwork attribute,
            // it should register this, but unfortunately, we've reached AutoFac's
            // limitations. 

            // The reason we're only registering NetListener when needed is
            // that it implements IStoppable and since we have no way of 
            // figuring out if NetListener is ALREADY instantiated from a 
            // constructor, later this will cause problems if we need to
            // get all instances of IStoppable. Hence we only need to 
            // register it if needed.
            if (new Type[] { typeof(InputFrom), typeof(PredictionConsumer) }
                    .Select(x => x.IsDefined(typeof(RequiresNetwork), true))
                    .FirstOrDefault(x => x))
                // If the input is from the network, then add the NetListener class.
                FullyRegister<NetListener>(); // Network handler class.

            return builder.Build();
        }


        public void RegisterActiveClasses()
        {
            // Setting the current active classes here.
            FullyRegister<Detector>(); // Detector class.

            FullyRegister<InputFrom>(); // Vision class.

            FullyRegister<PredictionConsumer>(); // Prediction handler class.

            FullyRegister<Astral<IDetectorService, IInputImage>>(); // Add the main class.
        }

        public void RegisterAll<T>() where T : notnull =>
             builder.RegisterAssemblyTypes(currentAssembly)
                .AssignableTo<T>()
                .SingleInstance();

        public void FullyRegister<T>() where T : notnull =>
            builder.RegisterType<T>()
                .AsImplementedInterfaces()
                .AsSelf()
                .SingleInstance(); // Detector class.

        public void RegisterLogger()
        {
            var appConfiguration = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("./appsettings.json").Build();

            // Set logger.
            Log.Logger = new LoggerConfiguration()
                    .ReadFrom.Configuration(appConfiguration)
                    .CreateLogger();

            builder.RegisterLogger();
        }
    }
}
