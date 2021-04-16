using System;
using System.Windows;

namespace TinyLittleMvvm {
    /// <summary>
    /// Defines the attached property <see cref="ServiceProviderProperty"/>.
    /// </summary>
    /// <remarks>
    /// This property is used to attach a <see cref="IServiceProvider"/>
    /// to a <see cref="DependencyObject"/>.
    /// </remarks>
    public static class ServiceProviderPropertyExtension {
        /// <summary>
        /// Defines a dependency property to attach a <see cref="IServiceProvider"/>
        /// to a <see cref="DependencyObject"/>
        /// </summary>
        public static readonly DependencyProperty ServiceProviderProperty = DependencyProperty.RegisterAttached(
            "ServiceProvider", typeof(IServiceProvider), typeof(ServiceProviderPropertyExtension), new PropertyMetadata(default(IServiceProvider)));

        /// <summary>
        /// Attaches a <see cref="IServiceProvider"/> to <paramref name="element"/>.
        /// </summary>
        /// <param name="element">The dependency object.</param>
        /// <param name="value">The <see cref="IServiceProvider"/>to attach to <paramref name="element"/>.</param>
        public static void SetServiceProvider(DependencyObject element, IServiceProvider? value) {
            element.SetValue(ServiceProviderProperty, value);
        }

        /// <summary>
        /// Gets the <see cref="IServiceProvider"/> attached to <paramref name="element"/>.
        /// </summary>
        /// <param name="element"></param>
        public static IServiceProvider? GetServiceProvider(DependencyObject element) {
            return (IServiceProvider) element.GetValue(ServiceProviderProperty);
        }
    }
}