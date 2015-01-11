using System.Threading.Tasks;

namespace TinyLittleMvvm {
    /// <summary>
    /// Declares the methods for showing <a href="http://mahapps.com/controls/flyouts.html">flyouts</a>.
    /// </summary>
    public interface IFlyoutManager {
        /// <summary>
        /// Shows a flyout for the given view model.
        /// </summary>
        /// <param name="viewModel">The view model for the flyout to be displayed.</param>
        /// <returns>A <see cref="Task"/> object which is completed when the flyout is closed.</returns>
        Task ShowFlyout(DialogViewModel viewModel);

        /// <summary>
        /// Shows a flyout for the given view model type.
        /// </summary>
        /// <typeparam name="TViewModel">The type of the view model.</typeparam>
        /// <returns>A <see cref="Task"/> object which is completed when the flyout is closed.</returns>
        Task ShowFlyout<TViewModel>() where TViewModel : DialogViewModel;

        /// <summary>
        /// Shows a flyout for the given view model.
        /// </summary>
        /// <param name="viewModel">The view model for the flyout to be displayed.</param>
        /// <typeparam name="TResult">The type of the flyout's result</typeparam>
        /// <returns>A <see cref="Task{TResult}"/> object promising the result of the view model when the flyout is closed.</returns>
        Task<TResult> ShowFlyout<TResult>(DialogViewModel<TResult> viewModel);

        /// <summary>
        /// Shows a flyout for the given view model type.
        /// </summary>
        /// <typeparam name="TViewModel">The type of the view model.</typeparam>
        /// <typeparam name="TResult">The type of the flyout's result</typeparam>
        /// <returns>A <see cref="Task{TResult}"/> object promising the result of the view model when the flyout is closed.</returns>
        Task<TResult> ShowFlyout<TViewModel, TResult>() where TViewModel : DialogViewModel<TResult>;
    }
}