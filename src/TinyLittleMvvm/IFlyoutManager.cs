using System.Threading.Tasks;

namespace TinyLittleMvvm {
    public interface IFlyoutManager {
        Task ShowFlyout(DialogViewModel viewModel);
        Task<TResult> ShowFlyout<TResult>(DialogViewModel<TResult> viewModel); 
    }
}