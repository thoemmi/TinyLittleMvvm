using System.Threading.Tasks;
using System.Windows;

namespace TinyLittleMvvm {
    /// <summary>
    /// This interface can be implemented by view models, which want to be notified when
    /// the corresponding view was loaded.
    /// </summary>
    public interface IOnLoadedHandler {
        /// <summary>
        /// This method is called asynchronously when the corresponding view's
        /// <see cref="FrameworkElement.Loaded"/> event was raised.
        /// </summary>
        /// <returns></returns>
        Task OnLoadedAsync();
    }
}