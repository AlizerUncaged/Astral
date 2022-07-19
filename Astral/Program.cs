﻿using Autofac;
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

            builder.RegisterType<Astral<Detection.YoloV5>>()
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