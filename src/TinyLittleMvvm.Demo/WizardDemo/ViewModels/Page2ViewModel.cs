using System;
using System.Threading.Tasks;
using TinyLittleMvvm.Wizard.ViewModels;

namespace TinyLittleMvvm.Demo.WizardDemo.ViewModels {
    public class Page2ViewModel : WizardPageViewModel {
        private bool _isConnecting;

        public override bool CanGoNext => !IsConnecting;

        public bool IsConnecting {
            get { return _isConnecting; }
            set {
                if (_isConnecting != value) {
                    _isConnecting = value;
                    NotifyOfPropertyChange(nameof(IsConnecting));
                }
            }
        }

        public override async Task OnEnterForwardAsync() {
            IsConnecting = true;
            await Task.Delay(TimeSpan.FromSeconds(2));
            IsConnecting = false;
            GoNext();
        }

        public override Task OnEnterBackwardAsync() {
            GoBack();
            return base.OnEnterBackwardAsync();
        }
    }
}