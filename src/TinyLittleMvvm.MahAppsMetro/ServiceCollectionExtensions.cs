using System;
using Microsoft.Extensions.DependencyInjection;

namespace TinyLittleMvvm.MahAppsMetro {
    /// <summary>
    /// IServiceCollection extension methods for common scenarios.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds required services to the given service collection.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="configure">An optional delegate to customize the resolution of ViewModels.</param>
        /// <returns></returns>
        public static IServiceCollection AddTinyLittleMvvmForMahAppsMetro(this IServiceCollection services, Action<ViewLocatorOptions> configure = null)
        {
            services.AddSingleton<IDialogManager, DialogManager>();
            services.AddSingleton<IFlyoutManager, FlyoutManager>();

            return services;
        }
    }
}