using System.Linq;
using System.Windows;
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
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IServiceCollection AddTinyLittleMvvm(this IServiceCollection services) {
            services.AddSingleton<ViewLocator>();
            services.AddSingleton<IWindowManager, WindowManager>();
            services.AddSingleton<IUiExecution, UiExecution>();

            return services;
        }

        /// <summary>
        /// Adds a transient viewmodel and its corresponding view to the service collection.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the types to.</param>
        /// <typeparam name="TViewModel">The type of the viewmodel to add.</typeparam>
        /// <typeparam name="TView">The type of the view to add.</typeparam>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IServiceCollection AddMvvmTransient<TViewModel, TView>(this IServiceCollection services) where TViewModel : class where TView : FrameworkElement
        {
            services
                .AddTransient<TViewModel>()
                .AddTransient<TView>();

            services
                .GetViewModelRegistry()
                .ViewModelTypeTo2ViewType.Add(typeof(TViewModel), typeof(TView));

            return services;
        }

        /// <summary>
        /// Adds a singleton viewmodel and its corresponding view to the service collection.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the types to.</param>
        /// <typeparam name="TViewModel">The type of the viewmodel to add.</typeparam>
        /// <typeparam name="TView">The type of the view to add.</typeparam>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IServiceCollection AddMvvmSingleton<TViewModel, TView>(this IServiceCollection services) where TViewModel : class where TView : FrameworkElement
        {
            services
                .AddSingleton<TViewModel>()
                .AddTransient<TView>();

            services
                .GetViewModelRegistry()
                .ViewModelTypeTo2ViewType.Add(typeof(TViewModel), typeof(TView));

            return services;
        }

        private static ViewModelRegistry GetViewModelRegistry(this IServiceCollection services)
        {
            ViewModelRegistry registry;
            var descriptor = services.FirstOrDefault(x => x.ServiceType == typeof(ViewModelRegistry));
            if (descriptor != null)
            {
                registry = (ViewModelRegistry)descriptor.ImplementationInstance!;
            }
            else
            {
                registry = new ViewModelRegistry();
                services.AddSingleton(registry);
            }

            return registry;
        }
    }
}