using System;
using Microsoft.Extensions.DependencyInjection;

namespace TinyLittleMvvm {
    /// <summary>
    /// IServiceCollection extension methods for common scenarios.
    /// </summary>
    public static class ServiceCollectionExtensions {
        /// <summary>
        /// Adds required services to the given service collection.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="configure">An optional delegate to customize the resolution of ViewModels.</param>
        /// <returns></returns>
        public static IServiceCollection AddTinyLittleMvvm(this IServiceCollection services, Action<ViewLocatorOptions> configure = null) {

            var options = new ViewLocatorOptions();
            configure?.Invoke(options);
            services.AddSingleton(options);

            services.AddSingleton<ViewLocator>();
            services.AddSingleton<IWindowManager, WindowManager>();
            services.AddSingleton<IUiExecution, UiExecution>();

            return services;
        }
        
    }
}