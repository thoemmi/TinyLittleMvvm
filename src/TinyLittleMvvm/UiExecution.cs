using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace TinyLittleMvvm {
    internal class UiExecution : IUiExecution {
        public void Execute(Action action) {
            var dispatcher = Application.Current.Dispatcher ?? Dispatcher.CurrentDispatcher;
            dispatcher.Invoke(action);
        }

        public Task ExecuteAsync(Action action) {
            var dispatcher = Application.Current.Dispatcher ?? Dispatcher.CurrentDispatcher;
            return dispatcher.InvokeAsync(action).Task;
        }
    }
}