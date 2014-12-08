using System;
using System.IO;
using System.Reflection;
using Autofac;
using TinyLittleMvvm.Demo.ViewModels;
using TinyLittleMvvm.Demo.Views;

namespace TinyLittleMvvm.Demo {
    public class AppBootstrapper : BootstrapperBase<IShell> {
        protected override void ConfigureContainer(ContainerBuilder builder) {
            base.ConfigureContainer(builder);

            builder.RegisterType<MainViewModel>().AsSelf().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<MainView>().SingleInstance();
            builder.RegisterType<SampleDialogView>().InstancePerDependency().AsSelf();
        }

        protected override string GetLogFolder() {
            var logFolder = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                Assembly.GetEntryAssembly().GetName().Name,
                "Logs");

            if (!Directory.Exists(logFolder)) {
                Directory.CreateDirectory(logFolder);
            }

            return logFolder;
        }
    }
}