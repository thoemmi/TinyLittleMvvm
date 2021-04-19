using System.Windows;
using Microsoft.Extensions.DependencyInjection;

namespace TinyLittleMvvm {
    /// <summary>
    /// Declares methods to show windows.
    /// </summary>
    public interface IWindowManager {
        /// <summary>
        /// Shows a window for a given view model type.
        /// </summary>
        /// <param name="owningWindow">An optional owner for the new window.</param>
        /// <param name="scope">Optional IoC scope for the window.</param>
        /// <typeparam name="TViewModel">The type of the view model.</typeparam>
        /// <returns>The window.</returns>
        Window ShowWindow<TViewModel>(Window? owningWindow = null, IServiceScope? scope = null);

        /// <summary>
        /// Shows a window for a given view model object.
        /// </summary>
        /// <param name="viewModel">The view model for the window to be displayed.</param>
        /// <param name="owningWindow">An optional owner for the new window.</param>
        /// <param name="scope">Optional IoC scope for the window.</param>
        /// <returns>The window.</returns>
        Window ShowWindow(object viewModel, Window? owningWindow = null, IServiceScope? scope = null);

        /// <summary>
        /// Shows a window for a given view model type as a Dialog.
        /// </summary>
        /// <param name="owningWindow">An optional owner for the new window.</param>
        /// <param name="scope">Optional IoC scope for the window.</param>
        /// <typeparam name="TViewModel">The type of the view model.</typeparam>
        /// <returns>A tuple composed of the result of <see cref="Window.ShowDialog()">ShowDialog</see> and the view model.</returns>
        (bool?, TViewModel) ShowDialog<TViewModel>(Window? owningWindow = null, IServiceScope? scope = null);

        /// <summary>
        /// Displays a message box.
        /// </summary>
        /// <param name="messageBoxText">A <see cref="string"/> that specifies the text to display.</param>
        /// <param name="button">A <see cref="MessageBoxButton"/> value that specifies which button or buttons to display</param>
        /// <param name="icon">A <see cref="MessageBoxImage"/> value that specifies the icon to display.</param>
        /// <returns>A <see cref="MessageBoxResult"/> value that specifies which message box button is clicked by the user.</returns>
        MessageBoxResult ShowMessageBox(string messageBoxText, MessageBoxButton button = MessageBoxButton.OK, MessageBoxImage icon = MessageBoxImage.None);

        /// <summary>
        /// Shuts down an application that returns the specified exit code to the operating system.
        /// </summary>
        /// <param name="exitCode">An integer exit code for an application. The default exit code is 0.</param>
        void ShutdownApplication(int exitCode = 0);
    }
}