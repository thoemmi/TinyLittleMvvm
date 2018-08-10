using MahApps.Metro.Controls;
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
        /// <param name="position">The flyout's position.</param>
        /// <returns>A <see cref="Task"/> object which is completed when the flyout is closed.</returns>
        Task ShowFlyout(DialogViewModel viewModel, Position position = Position.Right);

        /// <summary>
        /// Shows a flyout for the given view model type.
        /// </summary>
        /// <param name="position">The flyout's position.</param>
        /// <typeparam name="TViewModel">The type of the view model.</typeparam>
        /// <returns>A <see cref="Task"/> object which is completed when the flyout is closed.</returns>
        Task ShowFlyout<TViewModel>(Position position = Position.Right) where TViewModel : DialogViewModel;

        /// <summary>
        /// Shows a flyout for the given view model.
        /// </summary>
        /// <param name="viewModel">The view model for the flyout to be displayed.</param>
        /// <param name="position">The flyout's position.</param>
        /// <typeparam name="TResult">The type of the flyout's result</typeparam>
        /// <returns>A <see cref="Task{TResult}"/> object promising the result of the view model when the flyout is closed.</returns>
        Task<TResult> ShowFlyout<TResult>(DialogViewModel<TResult> viewModel, Position position = Position.Right);

        /// <summary>
        /// Shows a flyout for the given view model type.
        /// </summary>
        /// <param name="position">The flyout's position.</param>
        /// <typeparam name="TViewModel">The type of the view model.</typeparam>
        /// <typeparam name="TResult">The type of the flyout's result</typeparam>
        /// <returns>A <see cref="Task{TResult}"/> object promising the result of the view model when the flyout is closed.</returns>
        Task<TResult> ShowFlyout<TViewModel, TResult>(Position position = Position.Right) where TViewModel : DialogViewModel<TResult>;
    }
}