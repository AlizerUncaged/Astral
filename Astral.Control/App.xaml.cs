using Autofac;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;

namespace Astral.Control
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IContainer container;

        protected override void OnStartup(StartupEventArgs e)
        {
            var dataAccess = Assembly.GetExecutingAssembly();

            // Initialize dependency injections.
            var builder = new ContainerBuilder();
            
            // Add the utility functions.
            builder.RegisterAssemblyTypes(dataAccess)
                .AssignableTo<IUtility>()
                .AsImplementedInterfaces()
                .AsSelf()
                .InstancePerLifetimeScope();
            
            // Add the pages.
            builder.RegisterAssemblyTypes(dataAccess)
                .AssignableTo<IPage>()
                .AsImplementedInterfaces()
                .AsSelf()
                .InstancePerLifetimeScope();

            container = builder.Build();

            StartMainWindow();

        }
        private void StartMainWindow() =>
          container.BeginLifetimeScope()
            .Resolve<MainWindow>()
            .Show();
    }
}
