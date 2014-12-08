using System;

namespace TinyLittleMvvm {
    public interface IUiExecution {
        void Execute(Action action);
    }
}