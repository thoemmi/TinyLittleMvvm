using System;
using System.Threading.Tasks;

namespace TinyLittleMvvm {
    public abstract class DialogViewModel : PropertyChangedBase, IDialogViewModel {
        private readonly TaskCompletionSource<int> _tcs;

        protected DialogViewModel() {
            _tcs = new TaskCompletionSource<int>();
        }

        protected void Close() {
            _tcs.SetResult(0);

            var handler = Closed;
            if (handler != null) {
                handler(this, EventArgs.Empty);
            }
        }

        public Task Task {
            get { return _tcs.Task; }
        }

        public event EventHandler Closed;
    }

    public abstract class DialogViewModel<TResult> : PropertyChangedBase, IDialogViewModel {
        private readonly TaskCompletionSource<TResult> _tcs;

        protected DialogViewModel() {
            _tcs = new TaskCompletionSource<TResult>();
        }

        protected void Close(TResult result) {
            _tcs.SetResult(result);

            var handler = Closed;
            if (handler != null) {
                handler(this, EventArgs.Empty);
            }
        }

        public Task<TResult> Task {
            get { return _tcs.Task; }
        }

        public event EventHandler Closed;
    }
}