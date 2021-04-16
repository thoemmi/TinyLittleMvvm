using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace TinyLittleMvvm {
    /// <summary>
    /// Provides methods to get a view instance for a given view model.
    /// </summary>
    public class ViewLocator {
        private readonly IServiceProvider _serviceProvider;
        private readonly ViewModelRegistry _viewModelRegistry;
        private readonly ILogger<ViewLocator> _logger;

        /// <summary>
        /// Creates a new <see cref="ViewLocator"/> instance.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="logger">The logger.</param>
        public ViewLocator(IServiceProvider serviceProvider, ILogger<ViewLocator> logger) {
            _serviceProvider = serviceProvider;
            _viewModelRegistry = serviceProvider.GetRequiredService<ViewModelRegistry>();
            _logger = logger;
        }

        /// <summary>
        /// Gets the view for the passed view model.
        /// </summary>
        /// <param name="serviceProvider">The optional service provider. If <see langname="null"/>, the instance passed in the
        /// constructor will be used.</param>
        /// <typeparam name="TViewModel"></typeparam>
        /// <returns>The view matching the view model.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the view cannot be found in the IoC container.</exception>
        /// <remarks>
        /// <para>
        /// If <typeparamref name="TViewModel"/> implements <see cref="IOnLoadedHandler"/> or <see cref="IOnClosingHandler"/>,
        /// this method will register the view's corresponding events and call <see cref="IOnLoadedHandler.OnLoadedAsync"/>
        /// and <see cref="IOnClosingHandler.OnClosing"/> respectively when those events are raised.
        /// </para>
        /// <para>
        /// Don't call <strong>InitializeComponent()</strong> in your view's constructor yourself! If your view contains
        /// a method called InitializeComponent, this method will call it automatically via reflection.
        /// This allows the user of the library to remove the code-behind of her/his XAML files.
        /// </para>
        /// </remarks>
        public object GetViewForViewModel<TViewModel>(IServiceProvider? serviceProvider = null) {
            var viewModel = (serviceProvider ?? _serviceProvider).GetRequiredService<TViewModel>();
            return GetViewForViewModel(viewModel!, serviceProvider);
        }

        /// <summary>
        /// Gets the view for the passed view model.
        /// </summary>
        /// <param name="viewModel">The view model for which a view should be returned.</param>
        /// <param name="serviceProvider">The optional service provider. If <see langname="null"/>, the instance passed in the
        /// constructor will be used.</param>
        /// <returns>The view matching the view model.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the view cannot be found in the IoC container.</exception>
        /// <remarks>
        /// <para>
        /// If the <paramref name="viewModel"/> implements <see cref="IOnLoadedHandler"/> or <see cref="IOnClosingHandler"/>,
        /// this method will register the view's corresponding events and call <see cref="IOnLoadedHandler.OnLoadedAsync"/>
        /// and <see cref="IOnClosingHandler.OnClosing"/> respectively when those events are raised.
        /// </para>
        /// <para>
        /// Don't call <strong>InitializeComponent()</strong> in your view's constructor yourself! If your view contains
        /// a method called InitializeComponent, this method will call it automatically via reflection.
        /// This allows the user of the library to remove the code-behind of her/his XAML files.
        /// </para>
        /// </remarks>
        public object GetViewForViewModel(object viewModel, IServiceProvider? serviceProvider = null) {
            _logger.LogDebug($"View for view model {viewModel.GetType()} requested");
            if (!_viewModelRegistry.ViewModelTypeTo2ViewType.TryGetValue(viewModel.GetType(), out var viewType)) {
                _logger.LogError($"Could not find view for view model type {viewModel.GetType()}");
                throw new InvalidOperationException("No View found for ViewModel of type " + viewModel.GetType());
            }

            var view = _serviceProvider.GetRequiredService(viewType);
            _logger.LogDebug($"Resolved to instance of {view.GetType()}");

            if (serviceProvider != null && view is DependencyObject dependencyObject) {
                ServiceProviderPropertyExtension.SetServiceProvider(dependencyObject, serviceProvider);
            }

            if (view is FrameworkElement frameworkElement) {
                AttachHandler(frameworkElement, viewModel);
                frameworkElement.DataContext = viewModel;
            }

            InitializeComponent(view);

            return view;
        }

        private static void AttachHandler(FrameworkElement view, object viewModel) {
            if (viewModel is IOnLoadedHandler onLoadedHandler) {
                async void OnLoadedEvent(object sender, RoutedEventArgs args) {
                    view.Loaded -= OnLoadedEvent;
                    await onLoadedHandler.OnLoadedAsync();
                }

                view.Loaded += OnLoadedEvent;
            }

            if (viewModel is IOnClosingHandler onClosingHandler) {
                if (view is Window window) {
                    void OnClosingEvent(object sender, CancelEventArgs args) {
                        window.Closing -= OnClosingEvent;
                        onClosingHandler.OnClosing();
                    }

                    window.Closing += OnClosingEvent;
                } else {
                    void OnUnloadedEvent(object sender, RoutedEventArgs args) {
                        view.Unloaded -= OnUnloadedEvent;
                        onClosingHandler.OnClosing();
                    }

                    view.Unloaded += OnUnloadedEvent;
                }
            }

            if (viewModel is ICancelableOnClosingHandler cancelableOnClosingHandler) {
                if (!(view is Window window)) {
                    throw new ArgumentException("If a view model implements ICancelableOnClosingHandler, the corresponding view must be a window.");
                }

                void OnClosingEvent(object sender, CancelEventArgs args) {
                    args.Cancel = cancelableOnClosingHandler.OnClosing();
                }

                window.Closing += OnClosingEvent;

                void OnClosedHandler(object? sender, EventArgs args) {
                    window.Closing -= OnClosingEvent;
                    window.Closed -= OnClosedHandler;
                }

                window.Closed += OnClosedHandler;
            }
        }

        private static void InitializeComponent(object element) {
            var method = element.GetType().GetMethod("InitializeComponent", BindingFlags.Instance | BindingFlags.Public);
            method?.Invoke(element, null);
        }
    }
}