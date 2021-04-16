using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using MahApps.Metro.Controls;
using Microsoft.Extensions.DependencyInjection;

namespace TinyLittleMvvm.MahAppsMetro {
    internal class FlyoutManager : ObservableCollection<Flyout>, IFlyoutManager {
        private readonly ViewLocator _viewLocator;
        private readonly IServiceProvider _serviceProvider;

        public FlyoutManager(ViewLocator viewLocator, IServiceProvider serviceProvider) {
            _viewLocator = viewLocator;
            _serviceProvider = serviceProvider;
        }

        public Task ShowFlyout(DialogViewModel viewModel, Position position = Position.Right) {
            ShowFlyoutInternal(viewModel, position);
            return viewModel.Task;
        }

        public Task ShowFlyout<TViewModel>(Position position = Position.Right) where TViewModel : DialogViewModel {
            var viewModel = _serviceProvider.GetRequiredService<TViewModel>();
            ShowFlyoutInternal(viewModel, position);
            return viewModel.Task;
        }

        public Task<TResult> ShowFlyout<TResult>(DialogViewModel<TResult> viewModel, Position position = Position.Right) {
            ShowFlyoutInternal(viewModel, position);
            return viewModel.Task;
        }

        public Task<TResult> ShowFlyout<TViewModel, TResult>(Position position = Position.Right) where TViewModel : DialogViewModel<TResult> {
            var viewModel = _serviceProvider.GetRequiredService<TViewModel>();
            ShowFlyoutInternal(viewModel, position);
            return viewModel.Task;
        }

        private void ShowFlyoutInternal(IDialogViewModel viewModel, Position position) {
            var view = (FrameworkElement)_viewLocator.GetViewForViewModel(viewModel);

            var flyout = view as Flyout ?? new Flyout { Content = view };
            flyout.IsOpen = true;
            flyout.Position = position;
            flyout.IsModal = true;
            view.HorizontalAlignment = HorizontalAlignment.Left;

            flyout.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Controls.FlatButton.xaml") });

            void OnClosedEvent(object? sender, EventArgs args) {
                viewModel.Closed -= OnClosedEvent;
                flyout.IsOpen = false;
            }

            viewModel.Closed += OnClosedEvent;

            void OnClosingFinishedEvent(object o, RoutedEventArgs eventArgs) {
                flyout.ClosingFinished -= OnClosingFinishedEvent;
                Remove(flyout);
            }

            flyout.ClosingFinished += OnClosingFinishedEvent;

            Add(flyout);
        }
    }
}