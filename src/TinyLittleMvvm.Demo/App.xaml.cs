using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TinyLittleMvvm.Demo.ViewModels;
using TinyLittleMvvm.Demo.Views;

namespace TinyLittleMvvm.Demo {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {
        public ServiceProvider ServiceProvider { get; private set; }

        protected override void OnStartup(StartupEventArgs e) {
            base.OnStartup(e);

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            ServiceProvider = serviceCollection.BuildServiceProvider();

            ServiceProvider.GetService<IWindowManager>().ShowWindow<MainViewModel>();
        }

        protected override void OnExit(ExitEventArgs e) {
            base.OnExit(e);

            ServiceProvider.Dispose();
        }

        private void ConfigureServices(IServiceCollection services) {
            services
                .AddLogging(configure => configure.AddDebug())
                .Configure<LoggerFilterOptions>(options => options.MinLevel = LogLevel.Debug);

            services.AddTinyLittleMvvm();

            services.AddSingleton<MainView>();
            services.AddSingleton<MainViewModel>();

            services.AddTransient<SampleDialogView>();
            services.AddTransient<SampleDialogViewModel>();

            services.AddTransient<SampleFlyoutView>();
            services.AddTransient<SampleFlyoutViewModel>();

            services.AddTransient<SampleSubView>();
            services.AddTransient<SampleSubViewModel>();
        }
    }
}
