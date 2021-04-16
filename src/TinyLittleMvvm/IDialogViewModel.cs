using System;

namespace TinyLittleMvvm {
    /// <summary>
    /// Interface for dialog view models.
    /// </summary>
    public interface IDialogViewModel {
        /// <summary>
        /// This event is raised when the dialog was closed.
        /// </summary>
        event EventHandler? Closed;
    }
}