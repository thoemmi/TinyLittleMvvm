using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Extensions.DependencyInjection;

namespace TinyLittleMvvm {
    internal class DialogManager : IDialogManager {
        private readonly IServiceProvider _serviceProvider;
        private readonly ViewLocator _viewLocator;

        public DialogManager(ViewLocator viewLocator, IServiceProvider serviceProvider) {
            _viewLocator = viewLocator;
            _serviceProvider = serviceProvider;
        }

        public async Task ShowDialogAsync(DialogViewModel viewModel, MetroDialogSettings settings = null, IServiceScope serviceScope = null) {
            var view = _viewLocator.GetViewForViewModel(viewModel, serviceScope?.ServiceProvider);

            var dialog = view as BaseMetroDialog;
            if (dialog == null) {
                throw new InvalidOperationException($"The view {view.GetType()} belonging to view model {viewModel.GetType()} does not inherit from {typeof(BaseMetroDialog)}");
            }

            dialog.Resources.MergedDictionaries.Add(new ResourceDictionary {
                Source = new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Controls.FlatButton.xaml")
            });

            var firstMetroWindow = Application.Current.Windows.OfType<MetroWindow>().First();
            await firstMetroWindow.ShowMetroDialogAsync(dialog, settings);
            await viewModel.Task;
            await firstMetroWindow.HideMetroDialogAsync(dialog, settings);
        }

        public Task ShowDialogAsync<TViewModel>(MetroDialogSettings settings = null, IServiceScope serviceScope = null) where TViewModel : DialogViewModel {
            var viewModel = (serviceScope.ServiceProvider ?? _serviceProvider).GetRequiredService<TViewModel>();
            return ShowDialogAsync(viewModel, settings, serviceScope);
        }

        public async Task<TResult> ShowDialogAsync<TResult>(DialogViewModel<TResult> viewModel, MetroDialogSettings settings = null, IServiceScope serviceScope = null) {
            var view = _viewLocator.GetViewForViewModel(viewModel, serviceScope?.ServiceProvider ?? _serviceProvider);

            if (!(view is BaseMetroDialog dialog)) {
                throw new InvalidOperationException($"The view {view.GetType()} belonging to view model {viewModel.GetType()} does not inherit from {typeof(BaseMetroDialog)}");
            }

            dialog.Resources.MergedDictionaries.Add(new ResourceDictionary {
                Source = new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Controls.FlatButton.xaml")
            });

            var firstMetroWindow = Application.Current.Windows.OfType<MetroWindow>().First();
            await firstMetroWindow.ShowMetroDialogAsync(dialog, settings);
            var result = await viewModel.Task;
            await firstMetroWindow.HideMetroDialogAsync(dialog, settings);

            return result;
        }

        public Task<TResult> ShowDialogAsync<TViewModel, TResult>(MetroDialogSettings settings = null, IServiceScope serviceScope = null) where TViewModel : DialogViewModel<TResult> {
            var viewModel = (serviceScope?.ServiceProvider ?? _serviceProvider).GetRequiredService<TViewModel>();
            return ShowDialogAsync(viewModel, settings, serviceScope);
        }

        public Task<MessageDialogResult> ShowMessageBox(string title, string message, MessageDialogStyle style = MessageDialogStyle.Affirmative, MetroDialogSettings settings = null) {
            var firstMetroWindow = Application.Current.Windows.OfType<MetroWindow>().First();
            return firstMetroWindow.ShowMessageAsync(title, message, style, settings);
        }
    }
}