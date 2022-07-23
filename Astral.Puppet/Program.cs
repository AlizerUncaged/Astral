using Autofac;
using AutofacSerilogIntegration;
using LiteNetLib;
using LiteNetLib.Utils;
using Microsoft.Extensions.Configuration;
using Serilog;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Reflection;

namespace Astral.Puppet
{
    internal class Program
    {
        private ILifetimeScope lifetimeScope;

        public void Start()
        {
            Console.CancelKeyPress += Closing;

            var currentAssembly = Assembly
                .GetExecutingAssembly();

            var builder = new ContainerBuilder();

            builder.RegisterType<Astral.Models.Configurations.ScreenConfig>()
                .AsImplementedInterfaces()
                .AsSelf()
                .SingleInstance();

            builder.RegisterType<Utilities.DefaultImageCompressor>()
                .AsImplementedInterfaces()
                .AsSelf()
                .SingleInstance();

            builder.RegisterType<Astral.Models.Configurations.NetworkConfig>()
                .AsImplementedInterfaces()
                .AsSelf()
                .SingleInstance();

            builder.RegisterType<Models.NetworkLock>()
                .AsImplementedInterfaces()
                .AsSelf()
                .SingleInstance();

            builder.RegisterType<Utilities.ForegroundWindow>()
                .AsImplementedInterfaces()
                .AsSelf()
                .SingleInstance();

            builder.RegisterType<Utilities.PositionCalculator>()
                .AsImplementedInterfaces()
                .AsSelf()
                .SingleInstance();

            builder.RegisterType<Curses.MouseControl>()
                .AsImplementedInterfaces()
                .AsSelf()
                .SingleInstance();


            builder.RegisterAssemblyTypes(currentAssembly)
                .AssignableTo<IService>()
                .AsSelf()
                .SingleInstance();

            RegisterLogger(builder);

            var container = builder.Build();

            Task waitingTask;

            using (lifetimeScope = container?.BeginLifetimeScope())
            {
                lifetimeScope?.Resolve<Networking.NetListener>().StartListening();
                lifetimeScope?.Resolve<Input.MouseConsumer>();
                waitingTask = lifetimeScope?.Resolve<Input.ActiveWindowGrab>().StartAsync();
            }

            waitingTask.GetAwaiter().GetResult();

            Console.WriteLine("Exited...");
        }

        private void Closing(object? sender, ConsoleCancelEventArgs e)
        {
            Console.WriteLine("Exiting...");

            lifetimeScope?.Resolve<Input.ActiveWindowGrab>().Stop();
            lifetimeScope?.Resolve<Networking.NetListener>().Stop();
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

        static void Main(string[] args) =>
            new Program().Start(); // An exception of System.OperationCanceledException
                                                                 // might get called after calling close.

    }
}