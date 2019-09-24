using System;
using System.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TinyLittleMvvm.Demo.ViewModels;
using TinyLittleMvvm.Demo.Views;

namespace TinyLittleMvvm.Demo {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {
        private readonly IHost _host;

        public App() {
            _host = new HostBuilder()
                .ConfigureAppConfiguration((context, configurationBuilder) => {
                    configurationBuilder.SetBasePath(context.HostingEnvironment.ContentRootPath);
                    configurationBuilder.AddJsonFile("appsettings.json", optional: true);
                })
                .ConfigureServices((context, services) => {
                    services.AddTinyLittleMvvm();

                    ConfigureServices(services);
                })
                .ConfigureLogging(logging => {
                    logging.AddFilter("TinyLittleMvvm", LogLevel.Debug);
                    logging.AddDebug();
                })
                .Build();
        }

        private async void Application_Startup(object sender, StartupEventArgs e) {
            await _host.StartAsync();

            _host.Services
                .GetRequiredService<IWindowManager>()
                .ShowWindow<MainViewModel>();
        }

        private async void Application_Exit(object sender, ExitEventArgs e) {
            await _host.StopAsync(TimeSpan.FromSeconds(5));
            _host.Dispose();
        }

        private void ConfigureServices(IServiceCollection services) {
            services.AddSingleton<MainView>();
            services.AddSingleton<MainViewModel>();

            services.AddTransient<SampleDialogView>();
            services.AddTransient<SampleDialogViewModel>();

            services.AddTransient<SampleFlyoutView>();
            services.AddTransient<SampleFlyoutViewModel>();

            services.AddTransient<SampleSubView>();
            services.AddTransient<SampleSubViewModel>();

            services.AddScoped<ScopedService>();
        }
    }
}
