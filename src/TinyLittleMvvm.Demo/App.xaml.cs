using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
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
