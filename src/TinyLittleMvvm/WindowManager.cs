using System;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;

namespace TinyLittleMvvm {
    internal class WindowManager : IWindowManager {
        private readonly IServiceProvider _serviceProvider;
        private readonly ViewLocator _viewLocator;

        public WindowManager(IServiceProvider serviceProvider, ViewLocator viewLocator) {
            _serviceProvider = serviceProvider;
            _viewLocator = viewLocator;
        }

        public Window ShowWindow<TViewModel>(Window? owningWindow = null, IServiceScope? scope = null) {
            var serviceProvider = scope?.ServiceProvider
                                  ?? (owningWindow != null ? ServiceProviderPropertyExtension.GetServiceProvider(owningWindow) : null)
                                  ?? _serviceProvider;

            var window = (Window)_viewLocator.GetViewForViewModel<TViewModel>(serviceProvider);
            window.Owner = owningWindow;
            window.Show();
            return window;
        }

        public Window ShowWindow(object viewModel, Window? owningWindow = null, IServiceScope? scope = null) {
            var serviceProvider = scope?.ServiceProvider
                                  ?? (owningWindow != null ? ServiceProviderPropertyExtension.GetServiceProvider(owningWindow) : null)
                                  ?? _serviceProvider;

            var window = (Window)_viewLocator.GetViewForViewModel(viewModel, serviceProvider);
            window.Owner = owningWindow;
            window.Show();
            return window;
        }

        public (bool?, TViewModel) ShowDialog<TViewModel>(Window? owningWindow = null, IServiceScope? scope = null) {
            var serviceProvider = scope?.ServiceProvider
                                  ?? (owningWindow != null ? ServiceProviderPropertyExtension.GetServiceProvider(owningWindow) : null)
                                  ?? _serviceProvider;

            var viewModel = serviceProvider.GetRequiredService<TViewModel>()!;

            var window = (Window)_viewLocator.GetViewForViewModel(viewModel, serviceProvider);
            window.Owner = owningWindow;
            var result = window.ShowDialog();
            return (result, viewModel);
        }
    }
}
