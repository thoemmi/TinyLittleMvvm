using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Autofac;
using NLog;
using NLog.Config;
using NLog.Layouts;
using NLog.Targets;

namespace TinyLittleMvvm {
    public abstract class BootstrapperBase {
        protected BootstrapperBase() {
            Start();

            Application.Current.Startup += OnStartup;
            Application.Current.Exit += OnExit;
        }

        private void Start() {
            InitializeLogging();

            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
                LogManager.GetCurrentClassLogger().Error("UnhandledException", args.ExceptionObject as Exception);
            Application.Current.DispatcherUnhandledException += (sender, args) =>
                LogManager.GetCurrentClassLogger().Error("DispatcherUnhandledException", args.Exception);
            TaskScheduler.UnobservedTaskException += (sender, args) =>
                LogManager.GetCurrentClassLogger().Error("UnobservedTaskException", args.Exception);
            Application.Current.DispatcherUnhandledException += (sender, args) =>
                LogManager.GetCurrentClassLogger().Error("DispatcherUnhandledException", args.Exception);

            Container = CreateContainer();
        }

        private void InitializeLogging() {
            var config = new LoggingConfiguration();

            var fileTarget = new FileTarget {
                FileName = Path.Combine(GetLogFolder(), "log.xml"),
                ArchiveFileName = "log_{#####}.xml",
                ArchiveNumbering = ArchiveNumberingMode.Sequence,
                ArchiveAboveSize = 1024*1024,
                Layout = new Log4JXmlEventLayout()
            };
            config.AddTarget("file", fileTarget);
            config.LoggingRules.Add(new LoggingRule("*", LogLevel.Debug, fileTarget));

            var debuggerTarget = new DebuggerTarget();
            config.AddTarget("debugger", debuggerTarget);
            config.LoggingRules.Add(new LoggingRule("*", LogLevel.Debug, debuggerTarget));

            ConfigureLogging(config);

            LogManager.Configuration = config;

            PresentationTraceSources.DataBindingSource.Listeners.Add(new NLogTraceListener());
        }

        private IContainer CreateContainer() {
            var builder = new ContainerBuilder();

            builder.RegisterType<WindowManager>().SingleInstance();
            builder.RegisterType<DialogManager>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<FlyoutManager>().AsImplementedInterfaces().SingleInstance();

            ConfigureContainer(builder);

            var container = builder.Build();
            return container;
        }

        protected internal static IContainer Container { get; private set; }

        protected abstract string GetLogFolder();

        protected virtual void ConfigureContainer(ContainerBuilder builder) {
        }

        protected virtual void ConfigureLogging(LoggingConfiguration loggingConfiguration) {
        }

        protected virtual void OnStartup(object sender, StartupEventArgs e) {
        }

        protected virtual void OnExit(object sender, ExitEventArgs e) {
            Container.Dispose();
        }
    }

    public abstract class BootstrapperBase<TViewModel> : BootstrapperBase, IUiExecution {
        private Window _window;
        protected override void ConfigureContainer(ContainerBuilder builder) {
            base.ConfigureContainer(builder);
            builder.RegisterInstance(this).As<IUiExecution>();
        }

        protected override void OnStartup(object sender, StartupEventArgs e) {
            _window = Container.Resolve<WindowManager>().ShowWindow<TViewModel>();
        }

        public void Execute(Action action) {
            var dispatcher = _window != null ? _window.Dispatcher : Dispatcher.CurrentDispatcher;
            dispatcher.Invoke(action);
        }
    }
}