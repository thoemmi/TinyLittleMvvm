using System.Windows;
using TinyLittleMvvm.Wizard.ViewModels;

namespace TinyLittleMvvm {
    /// <summary>
    /// Declares methods to show windows.
    /// </summary>
    public interface IWindowManager {
        /// <summary>
        /// Shows a window for a given view model type.
        /// </summary>
        /// <param name="owningWindow">An optional owner for the new window.</param>
        /// <typeparam name="TViewModel">The type of the view model.</typeparam>
        /// <returns>The window.</returns>
        Window ShowWindow<TViewModel>(Window owningWindow = null);

        /// <summary>
        /// Shows a window for a given view model object.
        /// </summary>
        /// <param name="viewModel">The view model for the window to be displayed.</param>
        /// <param name="owningWindow">An optional owner for the new window.</param>
        /// <returns>The window.</returns>
        Window ShowWindow(object viewModel, Window owningWindow = null);

        /// <summary>
        /// Shows a wizard window.
        /// </summary>
        /// <param name="pageViewModels">The models of the wizard pages.</param>
        /// <typeparam name="TWizardViewModel">The type of the wiard view model.</typeparam>
        /// <returns>The window containing the wizard.</returns>
        Window ShowWizard<TWizardViewModel>(params WizardPageViewModel[] pageViewModels) where TWizardViewModel : WizardViewModel;

        /// <summary>
        /// Shows a wizard window.
        /// </summary>
        /// <param name="title">The title of the wizard window.</param>
        /// <param name="pageViewModels">The models of the wizard pages.</param>
        /// <returns>The window containing the wizard.</returns>
        Window ShowWizard(string title, params WizardPageViewModel[] pageViewModels);
    }
}