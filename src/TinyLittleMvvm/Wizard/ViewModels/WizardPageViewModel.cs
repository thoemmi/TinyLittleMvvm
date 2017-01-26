using System;
using System.Threading.Tasks;

namespace TinyLittleMvvm.Wizard.ViewModels {
    /// <summary>
    /// Base class for wizard page view models.
    /// </summary>
    public abstract class WizardPageViewModel : ValidationPropertyChangedBase {
        /// <summary>
        /// Goes to the next page.
        /// </summary>
        protected void GoNext() {
            InternalGoNext(this, EventArgs.Empty);
        }

        /// <summary>
        /// Goes to the previous page.
        /// </summary>
        protected void GoBack() {
            InternalGoBack(this, EventArgs.Empty);
        }

        /// <summary>
        /// Specifies whether this page allows going backward.
        /// </summary>
        public virtual bool CanGoBack => true;

        /// <summary>
        /// Specifies whether this page allows going forward.
        /// </summary>
        public virtual bool CanGoNext => !HasErrors;

        /// <summary>
        /// Specifies whether this page allows cancelling. If it is <see langword="null"/>, cancelling
        /// is allowed.
        /// </summary>
        public virtual bool? CanCancel { get; }

        /// <summary>
        /// Specifies whether this page allows finish. If it is <see langword="null"/>, finishing is
        /// allowed, if the page is the last page.
        /// </summary>
        public virtual bool? CanFinish { get; }

        /// <summary>
        /// Specifies whether the Cancel button should be visible. If it is <see langword="null"/>, the
        /// Cancel button is visible only if the page is not the last page.
        /// </summary>
        public virtual bool? IsCancelVisible => null;

        /// <summary>
        /// Specifies whether the Finish button should be visible. If it is <see langword="null"/>, the
        /// Finish button is visible only if the page is the last page.
        /// </summary>
        public virtual bool? IsFinishVisible => null;

        internal event EventHandler InternalGoNext;
        internal event EventHandler InternalGoBack;

        /// <summary>
        /// Called when the page was entered going forward.
        /// </summary>
        /// <returns></returns>
        public virtual Task OnEnterForwardAsync() => Task.FromResult(0);

        /// <summary>
        /// Called when the page was entered going backward.
        /// </summary>
        /// <returns></returns>
        public virtual Task OnEnterBackwardAsync() => Task.FromResult(0);
    }
}