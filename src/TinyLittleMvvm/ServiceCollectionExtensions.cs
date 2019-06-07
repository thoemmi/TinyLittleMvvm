using Microsoft.Extensions.DependencyInjection;

namespace TinyLittleMvvm {
    public static class ServiceCollectionExtensions {
        public static IServiceCollection AddTinyLittleMvvm(this IServiceCollection services) {

            services.AddSingleton<ViewLocator>();
            services.AddSingleton<IDialogManager, DialogManager>();
            services.AddSingleton<IWindowManager, WindowManager>();
            services.AddSingleton<IFlyoutManager, FlyoutManager>();
            services.AddSingleton<IUiExecution, UiExecution>();

            return services;
        }
        
    }
}