using Autofac;
using AutofacSerilogIntegration;
using LiteNetLib;
using LiteNetLib.Utils;
using Microsoft.Extensions.Configuration;
using Serilog;
using System.Net;
using System.Net.Sockets;
using System.Reflection;

namespace Astral.Puppet
{
    internal class Program
    {
        public async Task StartAsync()
        {
            var currentAssembly = Assembly
                .GetExecutingAssembly();

            var builder = new ContainerBuilder();

            builder.RegisterType<Astral.Models.ScreenConfig>()
                .AsImplementedInterfaces()
                .AsSelf()
                .SingleInstance();

            builder.RegisterType<Utilities.MonitorInfo>()
                .AsImplementedInterfaces()
                .AsSelf()
                .SingleInstance();


            builder.RegisterType<Utilities.DefaultImageCompressor>()
                .AsImplementedInterfaces()
                .AsSelf()
                .SingleInstance();

            builder.RegisterType<Astral.Models.NetworkConfig>()
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

            using (var lifetimeScope = container?.BeginLifetimeScope())
            {
                lifetimeScope?.Resolve<Networking.NetListener>().StartListening();
                lifetimeScope?.Resolve<Input.MouseConsumer>();
                await lifetimeScope?.Resolve<Input.ActiveWindowGrab>().StartAsync()!;
            }

            await Task.Delay(-1);

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
            new Program().StartAsync().GetAwaiter().GetResult();
    }
}