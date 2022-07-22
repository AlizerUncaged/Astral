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
        private CancellationTokenSource cancellationTokenSource =
            new CancellationTokenSource();

        private ILifetimeScope lifetimeScope;

        public async Task StartAsync()
        {
            Console.CancelKeyPress += Closing;

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

            using (lifetimeScope = container?.BeginLifetimeScope())
            {
                lifetimeScope?.Resolve<Networking.NetListener>().StartListening();
                lifetimeScope?.Resolve<Input.MouseConsumer>();
                await lifetimeScope?.Resolve<Input.ActiveWindowGrab>().StartAsync()!;
            }

            try
            {
                await Task.Delay(-1, cancellationTokenSource.Token);
            }
            catch { } // Cancel exception.

            Console.WriteLine("Exited...");
        }

        private void Closing(object? sender, ConsoleCancelEventArgs e)
        {
            Console.WriteLine("Exiting...");
            cancellationTokenSource.Cancel();

            lifetimeScope?.Resolve<Input.ActiveWindowGrab>().Stop();
            lifetimeScope?.Resolve<Networking.NetListener>().Stop();

            // Commit suicide.
            Process.GetCurrentProcess().Kill();
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