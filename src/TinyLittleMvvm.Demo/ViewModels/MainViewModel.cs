using System.Threading.Tasks;
using System.Windows.Input;

namespace TinyLittleMvvm.Demo.ViewModels {
    public class MainViewModel : PropertyChangedBase, IShell, IOnLoadedHandler {
        private readonly IDialogManager _dialogManager;
        private ICommand _showSampleDialogCommand;
        private string _title;

        public MainViewModel(IDialogManager dialogManager) {
            _dialogManager = dialogManager;
            _showSampleDialogCommand = new AsyncRelayCommand(OnShowSampleDialogAsync);
        }

        public Task OnLoadedAsync() {
            Title = "Tiny Little MVVM Demo";

            return Task.FromResult(0);
        }

        public string Title {
            get { return _title; }
            set {
                if (_title != value) {
                    _title = value;
                    NotifyOfPropertyChange(() => Title);
                }
            }
        }

        public ICommand ShowSampleDialogCommand {
            get { return _showSampleDialogCommand; }
        }

        private async Task OnShowSampleDialogAsync() {
            var text = await _dialogManager.ShowDialogAsync(new SampleDialogViewModel());
            if (text != null) {
                await _dialogManager.ShowMessageBox(Title, "You entered: " + text);
            }
        }
    }
}