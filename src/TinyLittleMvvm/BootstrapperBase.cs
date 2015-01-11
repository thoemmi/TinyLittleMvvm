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
    /// <summary>
    /// Base class for bootstrapper. DON'T INHERIT FROm THIS CLASS BUT <see cref="BootstrapperBase{TViewModel}"/>.
    /// </summary>
    public abstract class BootstrapperBase {
        /// <summary>
        /// Constructor.
        /// </summary>
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
            // ReSharper disable FormatStringProblem
            TaskScheduler.UnobservedTaskException += (sender, args) =>
                LogManager.GetCurrentClassLogger().Error("UnobservedTaskException", args.Exception);
            // ReSharper restore FormatStringProblem
            Application.Current.DispatcherUnhandledException += (sender, args) =>
                LogManager.GetCurrentClassLogger().Error("DispatcherUnhandledException", args.Exception);

            Container = CreateContainer();
        }

        private void InitializeLogging() {
            var config = new LoggingConfiguration();

            var fileTarget = new FileTarget {
                FileName = Path.Combine(GetLogFolder(), "log.xml"),
                ArchiveFileName = "log_{#####}.xml",
                ArchiveNumbering = ArchiveNumberingMode.Rolling,
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

        /// <summary>
        /// The Autofac container used by the application.
        /// </summary>
        protected internal static IContainer Container { get; private set; }

        /// <summary>
        /// This method is called to get the folder where the log files should be written.
        /// </summary>
        /// <returns>The folder where the log files should be written.</returns>
        protected abstract string GetLogFolder();

        /// <summary>
        /// This methods allows the inherited class to register her/his classes.
        /// </summary>
        /// <param name="builder"></param>
        protected virtual void ConfigureContainer(ContainerBuilder builder) {
        }

        /// <summary>
        /// When overroding this method, the implementor can configure additional logging targets.
        /// </summary>
        /// <param name="loggingConfiguration">The logging configuration.</param>
        /// <remarks>
        /// <para>
        /// TinyLittleMvvm already uses two targets:
        /// <ul>
        /// <li>A file logger, with up to 10 rolling files with 1 MB size each, to the folder specified by <see cref="GetLogFolder"/>, with log level Debug.</li>
        /// <li>A <a href="https://github.com/nlog/nlog/wiki/Debug-target">Debug target</a>, with log level Debug too.</li>
        /// </ul>
        /// </para>
        /// <para>
        /// See <a href="https://github.com/nlog/nlog/wiki/Configuration-API">NLog's Configuration API</a>
        /// on how to add additional targets.
        /// </para>
        /// </remarks>
        protected virtual void ConfigureLogging(LoggingConfiguration loggingConfiguration) {
        }

        /// <summary>
        /// Called when the Run method of the Application object is called.
        /// </summary>
        /// <param name="sender">The sender of the <see cref="Application.Startup"/> event.</param>
        /// <param name="e">Contains the arguments of the Startup event.</param>
        protected virtual void OnStartup(object sender, StartupEventArgs e) {
        }

        /// <summary>
        /// Called just before an application shuts down, and cannot be canceled.
        /// </summary>
        /// <param name="sender">The sender of the <see cref="Application.Exit"/> event.</param>
        /// <param name="e">Contains the arguments of the Exit event.</param>
        protected virtual void OnExit(object sender, ExitEventArgs e) {
            Container.Dispose();
        }
    }

    /// <summary>
    /// Base class for bootstrapper.
    /// </summary>
    /// <typeparam name="TViewModel">The type of the main window's view model.</typeparam>
    public abstract class BootstrapperBase<TViewModel> : BootstrapperBase, IUiExecution {
        private Window _window;

        /// <summary>
        /// This methods allows the inherited class to register her/his classes.
        /// </summary>
        /// <param name="builder"></param>
        protected override void ConfigureContainer(ContainerBuilder builder) {
            base.ConfigureContainer(builder);
            builder.RegisterInstance(this).As<IUiExecution>();
        }

        /// <summary>
        /// Called when the Run method of the Application object is called.
        /// </summary>
        /// <param name="sender">The sender of the <see cref="Application.Startup"/> event.</param>
        /// <param name="e">Contains the arguments of the Startup event.</param>
        protected override void OnStartup(object sender, StartupEventArgs e) {
            _window = Container.Resolve<WindowManager>().ShowWindow<TViewModel>();
        }

        /// <summary>
        /// Executes the passed action in the dispatcher thread.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        void IUiExecution.Execute(Action action) {
            var dispatcher = _window != null ? _window.Dispatcher : Dispatcher.CurrentDispatcher;
            dispatcher.Invoke(action);
        }
    }
}