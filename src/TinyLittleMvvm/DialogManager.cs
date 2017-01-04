using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Autofac;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace TinyLittleMvvm {
    internal class DialogManager : IDialogManager {
        public async Task ShowDialogAsync(DialogViewModel viewModel, MetroDialogSettings settings = null) {
            var view = ViewLocator.GetViewForViewModel(viewModel);

            var dialog = view as BaseMetroDialog;
            if (dialog == null) {
                throw new InvalidOperationException(String.Format("The view {0} belonging to view model {1} does not inherit from {2}",
                    view.GetType(), viewModel.GetType(), typeof (BaseMetroDialog)));
            }

            dialog.Resources.MergedDictionaries.Add(new ResourceDictionary {
                Source = new Uri("pack://application:,,,/MahApps.Metro;component/Styles/FlatButton.xaml")
            });

            var firstMetroWindow = Application.Current.Windows.OfType<MetroWindow>().First();
            await firstMetroWindow.ShowMetroDialogAsync(dialog, settings);
            await viewModel.Task;
            await firstMetroWindow.HideMetroDialogAsync(dialog, settings);
        }

        public Task ShowDialogAsync<TViewModel>(MetroDialogSettings settings = null) where TViewModel : DialogViewModel {
            var viewModel = BootstrapperBase.Container.Resolve<TViewModel>();
            return ShowDialogAsync(viewModel, settings);
        }

        public async Task<TResult> ShowDialogAsync<TResult>(DialogViewModel<TResult> viewModel, MetroDialogSettings settings = null) {
            var view = ViewLocator.GetViewForViewModel(viewModel);

            var dialog = view as BaseMetroDialog;
            if (dialog == null) {
                throw new InvalidOperationException(String.Format("The view {0} belonging to view model {1} does not inherit from {2}",
                    view.GetType(), viewModel.GetType(), typeof (BaseMetroDialog)));
            }

            dialog.Resources.MergedDictionaries.Add(new ResourceDictionary {
                Source = new Uri("pack://application:,,,/MahApps.Metro;component/Styles/FlatButton.xaml")
            });

            var firstMetroWindow = Application.Current.Windows.OfType<MetroWindow>().First();
            await firstMetroWindow.ShowMetroDialogAsync(dialog, settings);
            var result = await viewModel.Task;
            await firstMetroWindow.HideMetroDialogAsync(dialog, settings);

            return result;
        }

        public Task<TResult> ShowDialogAsync<TViewModel, TResult>(MetroDialogSettings settings = null) where TViewModel : DialogViewModel<TResult> {
            var viewModel = BootstrapperBase.Container.Resolve<TViewModel>();
            return ShowDialogAsync(viewModel, settings);
        }

        public Task<MessageDialogResult> ShowMessageBox(string title, string message, MessageDialogStyle style = MessageDialogStyle.Affirmative, MetroDialogSettings settings = null) {
            var firstMetroWindow = Application.Current.Windows.OfType<MetroWindow>().First();
            return firstMetroWindow.ShowMessageAsync(title, message, style, settings);
        }
    }
}