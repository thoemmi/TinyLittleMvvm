using System.Threading.Tasks;
using System.Windows.Input;

namespace TinyLittleMvvm.Wizard.ViewModels {
    public class WizardViewModel : PropertyChangedBase {
        public WizardViewModel() {
            BackCommand = new AsyncRelayCommand(OnBackAsync, CanBack);
            NextCommand = new AsyncRelayCommand(OnNextAsync, CanNext);
            CancelCommand = new AsyncRelayCommand(OnCancelAsync, CanCancel);
            FinishCommand = new AsyncRelayCommand(OnFinishAsync, CanFinish);
        }

        public ICommand BackCommand { get; }
        public ICommand NextCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand FinishCommand { get; }
        public bool IsCancelVisible { get; set; }
        public bool IsFinishVisible { get; set; }

        private bool CanBack() {
            return true;
        }

        private Task OnBackAsync() {
            return Task.FromResult(0);

        }
        private bool CanNext() {
            return true;
        }

        private Task OnNextAsync() {
            return Task.FromResult(0);
        }
        private bool CanCancel() {
            return true;
        }

        private Task OnCancelAsync() {
            return Task.FromResult(0);
        }
        private bool CanFinish() {
            return true;
        }

        private Task OnFinishAsync() {
            return Task.FromResult(0);
        }
    }
}