using System.Windows;

namespace TinyLittleMvvm {
    /// <summary>
    /// This interface can be implemented by view models, which want to be notified when
    /// the corresponding view is about to be closed.
    /// </summary>
    public interface IOnClosingHandler {
        /// <summary>
        /// This method is called when the corresponding view's <see cref="FrameworkElement.Loaded"/> event was raised.
        /// </summary>
        /// <returns></returns>
        void OnClosing();
    }
}