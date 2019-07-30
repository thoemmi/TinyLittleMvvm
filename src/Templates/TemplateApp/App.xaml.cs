using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TinyLittleMvvm;

using TemplateApp.ViewModels;
using TemplateApp.Views;

namespace TemplateApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public ServiceProvider ServiceProvider { get; private set; }

        protected override void OnStartup(StartupEventArgs e) {
            base.OnStartup(e);

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            ServiceProvider = serviceCollection.BuildServiceProvider();

            ServiceProvider.GetRequiredService<IWindowManager>().ShowWindow<MainViewModel>();
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
        }
    }
}
