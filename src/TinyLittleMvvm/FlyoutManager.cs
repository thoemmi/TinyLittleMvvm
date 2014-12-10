using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using MahApps.Metro.Controls;

namespace TinyLittleMvvm {
    public class FlyoutManager : ObservableCollection<Flyout>, IFlyoutManager {
        public Task ShowFlyout(DialogViewModel viewModel) {
            ShowFlyoutInternal(viewModel);
            return viewModel.Task;
        }

        public Task<TResult> ShowFlyout<TResult>(DialogViewModel<TResult> viewModel) {
            ShowFlyoutInternal(viewModel);
            return viewModel.Task;
        }

        private void ShowFlyoutInternal(IDialogViewModel viewModel) {
            var view = (FrameworkElement)ViewLocator.GetViewForViewModel(viewModel);

            var flyout = view as Flyout ?? new Flyout { Content = view };
            flyout.IsOpen = true;
            flyout.Position = Position.Right;
            flyout.IsModal = true;
            view.HorizontalAlignment = HorizontalAlignment.Left;

            flyout.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/MahApps.Metro;component/Styles/FlatButton.xaml") });

            EventHandler closedHandler = null;
            closedHandler = (sender, args) => {
                viewModel.Closed -= closedHandler;
                flyout.IsOpen = false;

            };
            viewModel.Closed += closedHandler;

            RoutedEventHandler closingFinishedHandler = null;
            closingFinishedHandler = (o, eventArgs) => {
                flyout.ClosingFinished -= closingFinishedHandler;
                Remove(flyout);
            };
            flyout.ClosingFinished += closingFinishedHandler;

            Add(flyout);
        }
    }
}