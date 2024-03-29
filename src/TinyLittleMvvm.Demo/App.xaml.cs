﻿using System;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TinyLittleMvvm.Demo.ViewModels;
using TinyLittleMvvm.Demo.Views;
using TinyLittleMvvm.MahAppsMetro;

namespace TinyLittleMvvm.Demo {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {
        private readonly IHost _host;

        public App() {
            _host = Host
                .CreateDefaultBuilder()
                .ConfigureServices((context, services) => {
                    services
                        .AddTinyLittleMvvm()
                        .AddTinyLittleMvvmForMahAppsMetro();

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
            services
                .AddMvvmSingleton<MainViewModel, MainView>()
                .AddMvvmTransient<SampleDialogViewModel, SampleDialogView>()
                .AddMvvmTransient<SampleFlyoutViewModel, SampleFlyoutView>()
                .AddMvvmTransient<SampleSubViewModel, SampleSubView>()
                .AddMvvmTransient<WindowViewModel, WindowView>();

            services.AddScoped<ScopedService>();
        }
    }
}
