using System;
using System.Windows;

namespace TinyLittleMvvm {
    public static class ServiceProviderPropertyExtension {
        public static readonly DependencyProperty ServiceProviderProperty = DependencyProperty.RegisterAttached(
            "ServiceProvider", typeof(IServiceProvider), typeof(ServiceProviderPropertyExtension), new PropertyMetadata(default(IServiceProvider)));

        public static void SetServiceProvider(DependencyObject element, IServiceProvider value) {
            element.SetValue(ServiceProviderProperty, value);
        }

        public static IServiceProvider GetServiceProvider(DependencyObject element) {
            return (IServiceProvider) element.GetValue(ServiceProviderProperty);
        }
    }
}