using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using Autofac;
using NLog;

namespace TinyLittleMvvm {
    public static class ViewLocator {
        private static readonly Logger _log = LogManager.GetCurrentClassLogger();

        public static Func<Type, Type> GetViewTypeFromViewModelType;
        public static Func<string, string> GetViewTypeNameFromViewModelTypeName;

        static ViewLocator() {
            GetViewTypeNameFromViewModelTypeName = viewModeltypeName => viewModeltypeName.Replace("ViewModel", "View");
            GetViewTypeFromViewModelType = type => {
                var viewModelTypeName = type.FullName;
                var viewTypeName = GetViewTypeNameFromViewModelTypeName(viewModelTypeName);
                var viewType = type.Assembly.GetType(viewTypeName);
                return viewType;
            };
        }

        public static object GetViewForViewModel<TViewModel>(ILifetimeScope lifetimeScope = null) {
            var viewModel = (lifetimeScope ?? BootstrapperBase.Container).Resolve(typeof(TViewModel));
            return GetViewForViewModel(viewModel);
        }

        public static object GetViewForViewModel(object viewModel) {
            _log.Debug("View for view model {0} requested", viewModel.GetType());
            var viewType = GetViewTypeFromViewModelType(viewModel.GetType());
            if (viewType == null) {
                _log.Error("Could not find view for view model type {0}", viewModel.GetType());
                throw new InvalidOperationException("No View found for ViewModel of type " + viewModel.GetType());
            }

            var view = BootstrapperBase.Container.Resolve(viewType);
            _log.Debug("Resolved to instance of {0}", view.GetType());

            var frameworkElement = view as FrameworkElement;
            if (frameworkElement != null) {
                AttachHandler(frameworkElement, viewModel);
                frameworkElement.DataContext = viewModel;
            }

            InitializeComponent(view);

            return view;
        }

        private static void AttachHandler(FrameworkElement view, object viewModel) {
            var onLoadedHandler = viewModel as IOnLoadedHandler;
            if (onLoadedHandler != null) {
                RoutedEventHandler handler = null;
                handler = async (sender, args) => {
                    view.Loaded -= handler;
                    await onLoadedHandler.OnLoadedAsync();
                };
                view.Loaded += handler;
            }

            var onClosingHandler = viewModel as IOnClosingHandler;
            if (onClosingHandler != null) {
                var window = view as Window;
                if (window != null) {
                    CancelEventHandler handler = null;
                    handler = (sender, args) => {
                        window.Closing -= handler;
                        onClosingHandler.OnClosing();
                    };
                    window.Closing += handler;
                } else {
                    RoutedEventHandler handler = null;
                    handler = (sender, args) => {
                        view.Unloaded -= handler;
                        onClosingHandler.OnClosing();
                    };
                    view.Unloaded += handler;
                }
            }
        }

        private static void InitializeComponent(object element) {
            var method = element.GetType().GetMethod("InitializeComponent", BindingFlags.Instance | BindingFlags.Public);
            if (method == null) {
                return;
            }
            method.Invoke(element, null);
        }
    }
}