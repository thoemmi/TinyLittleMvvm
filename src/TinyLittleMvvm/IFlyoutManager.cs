using System.Threading.Tasks;

namespace TinyLittleMvvm {
    public interface IFlyoutManager {
        Task ShowFlyout(DialogViewModel viewModel);
        Task ShowFlyout<TViewModel>() where TViewModel : DialogViewModel;
        Task<TResult> ShowFlyout<TResult>(DialogViewModel<TResult> viewModel);
        Task<TResult> ShowFlyout<TViewModel, TResult>() where TViewModel : DialogViewModel<TResult>;
    }
}