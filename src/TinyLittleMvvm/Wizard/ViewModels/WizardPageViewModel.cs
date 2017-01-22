using System;
using System.Threading.Tasks;

namespace TinyLittleMvvm.Wizard.ViewModels {
    public abstract class WizardPageViewModel : ValidationPropertyChangedBase {
        protected void GoNext() {
            InternalGoNext(this, EventArgs.Empty);
        }

        protected void GoBack() {
            InternalGoBack(this, EventArgs.Empty);
        }

        public virtual bool CanGoBack => true;
        public virtual bool CanGoNext => !HasErrors;
        public virtual bool? CanCancel { get; }
        public virtual bool? CanFinish { get; }
        public virtual bool? IsCancelVisible => null;
        public virtual bool? IsFinishVisible => null;

        internal event EventHandler InternalGoNext;
        internal event EventHandler InternalGoBack;

        public virtual Task OnEnterForwardAsync() => Task.FromResult(0);
        public virtual Task OnEnterBackwardAsync() => Task.FromResult(0);
    }
}