using System;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TemplateApp.MahAppsMetro.ViewModels;
using TemplateApp.MahAppsMetro.Views;
using TinyLittleMvvm;
using TinyLittleMvvm.MahAppsMetro;

namespace TemplateApp.MahAppsMetro {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App {
        private readonly IHost _host;

        public App() {
            _host = Host
                .CreateDefaultBuilder()
                .ConfigureServices((context, services) => {
                    ConfigureServices(services);
                })
                .ConfigureLogging(logging => {
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
            services.AddTinyLittleMvvm();
            services.AddTinyLittleMvvmForMahAppsMetro();

            services.AddMvvmSingleton<MainViewModel, MainView>();
        }
    }
}
