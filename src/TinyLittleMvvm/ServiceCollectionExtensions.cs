using System;
using Microsoft.Extensions.DependencyInjection;

namespace TinyLittleMvvm {
    public static class ServiceCollectionExtensions {
        public static IServiceCollection AddTinyLittleMvvm(this IServiceCollection services, Action<ViewLocatorOptions> configure = null) {

            var options = new ViewLocatorOptions();
            configure?.Invoke(options);
            services.AddSingleton(options);

            services.AddSingleton<ViewLocator>();
            services.AddSingleton<IDialogManager, DialogManager>();
            services.AddSingleton<IWindowManager, WindowManager>();
            services.AddSingleton<IFlyoutManager, FlyoutManager>();
            services.AddSingleton<IUiExecution, UiExecution>();

            return services;
        }
        
    }
}