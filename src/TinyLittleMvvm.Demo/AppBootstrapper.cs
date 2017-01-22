using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Autofac;
using NLog;
using NLog.Config;
using NLog.Layouts;
using NLog.Targets;
using TinyLittleMvvm.Demo.ViewModels;
using TinyLittleMvvm.Demo.Views;
using TinyLittleMvvm.Demo.WizardDemo.ViewModels;
using TinyLittleMvvm.Demo.WizardDemo.Views;

namespace TinyLittleMvvm.Demo {
    public class AppBootstrapper : BootstrapperBase<IShell> {
        public AppBootstrapper() {
            InitializeLogging();
        }

        protected override void ConfigureContainer(ContainerBuilder builder) {
            base.ConfigureContainer(builder);

            builder.RegisterType<MainViewModel>().AsSelf().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<MainView>().SingleInstance();

            builder.RegisterType<SampleDialogView>().InstancePerDependency().AsSelf();
            builder.RegisterType<SampleDialogViewModel>().InstancePerDependency().AsSelf();

            builder.RegisterType<SampleFlyoutView>().InstancePerDependency().AsSelf();
            builder.RegisterType<SampleFlyoutViewModel>().InstancePerDependency().AsSelf();

            builder.RegisterType<SampleSubView>().InstancePerDependency().AsSelf();
            builder.RegisterType<SampleSubViewModel>().InstancePerDependency().AsSelf();

            builder.RegisterType<Page1View>().InstancePerDependency();
            builder.RegisterType<Page1ViewModel>().InstancePerDependency();
            builder.RegisterType<Page2View>().InstancePerDependency();
            builder.RegisterType<Page2ViewModel>().InstancePerDependency();
            builder.RegisterType<Page3View>().InstancePerDependency();
            builder.RegisterType<Page3ViewModel>().InstancePerDependency();
        }

        private static void InitializeLogging() {
            var config = new LoggingConfiguration();

            var debuggerTarget = new DebuggerTarget();
            config.AddTarget("debugger", debuggerTarget);
            config.LoggingRules.Add(new LoggingRule("*", LogLevel.Debug, debuggerTarget));

            var logFolder = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                Assembly.GetEntryAssembly().GetName().Name,
                "Logs");

            if (!Directory.Exists(logFolder)) {
                Directory.CreateDirectory(logFolder);
            }

            var fileTarget = new FileTarget {
                FileName = Path.Combine(logFolder, "log.xml"),
                ArchiveFileName = "log_{#####}.xml",
                ArchiveNumbering = ArchiveNumberingMode.Rolling,
                ArchiveAboveSize = 1024 * 1024,
                Layout = new Log4JXmlEventLayout()
            };
            config.AddTarget("file", fileTarget);
            config.LoggingRules.Add(new LoggingRule("*", LogLevel.Debug, fileTarget));

            LogManager.Configuration = config;

            PresentationTraceSources.DataBindingSource.Listeners.Add(new NLogTraceListener());
        }
    }
}