using System.Threading.Tasks;
using MahApps.Metro.Controls.Dialogs;

namespace TinyLittleMvvm {
    public interface IDialogManager {
        Task ShowDialogAsync(DialogViewModel viewModel);
        Task ShowDialogAsync<TViewModel>() where TViewModel : DialogViewModel;
        Task<TResult> ShowDialogAsync<TResult>(DialogViewModel<TResult> viewModel);
        Task<TResult> ShowDialogAsync<TViewModel, TResult>() where TViewModel : DialogViewModel<TResult>;
        Task<MessageDialogResult> ShowMessageBox(string title, string message, MessageDialogStyle style = MessageDialogStyle.Affirmative, MetroDialogSettings settings = null);
    }
}