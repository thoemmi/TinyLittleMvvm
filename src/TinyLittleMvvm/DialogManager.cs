using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace TinyLittleMvvm {
    public class DialogManager : IDialogManager {
        public async Task ShowDialogAsync(DialogViewModel viewModel) {
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
            await firstMetroWindow.ShowMetroDialogAsync(dialog);
            await viewModel.Task;
            await firstMetroWindow.HideMetroDialogAsync(dialog);
        }

        public async Task<TResult> ShowDialogAsync<TResult>(DialogViewModel<TResult> viewModel) {
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
            await firstMetroWindow.ShowMetroDialogAsync(dialog);
            var result = await viewModel.Task;
            await firstMetroWindow.HideMetroDialogAsync(dialog);

            return result;
        }

        public Task<MessageDialogResult> ShowMessageBox(string title, string message, MessageDialogStyle style = MessageDialogStyle.Affirmative, MetroDialogSettings settings = null) {
            var firstMetroWindow = Application.Current.Windows.OfType<MetroWindow>().First();
            return firstMetroWindow.ShowMessageAsync(title, message, style, settings);
        }
    }
}