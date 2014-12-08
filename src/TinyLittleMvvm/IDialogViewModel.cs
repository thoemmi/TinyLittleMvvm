using System;

namespace TinyLittleMvvm {
    public interface IDialogViewModel {
        event EventHandler Closed;
    }
}