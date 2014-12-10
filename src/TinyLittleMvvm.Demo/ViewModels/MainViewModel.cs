using System.Threading.Tasks;
using System.Windows.Input;

namespace TinyLittleMvvm.Demo.ViewModels {
    public class MainViewModel : PropertyChangedBase, IShell, IOnLoadedHandler {
        private readonly IDialogManager _dialogManager;
        private readonly IFlyoutManager _flyoutManager;
        private readonly ICommand _showSampleDialogCommand;
        private readonly ICommand _showSampleFlyoutCommand;
        private string _title;

        public MainViewModel(IDialogManager dialogManager, IFlyoutManager flyoutManager) {
            _dialogManager = dialogManager;
            _flyoutManager = flyoutManager;
            _showSampleDialogCommand = new AsyncRelayCommand(OnShowSampleDialogAsync);
            _showSampleFlyoutCommand = new AsyncRelayCommand(OnShowSampleFlyoutAsync);
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

        public ICommand ShowSampleFlyoutCommand {
            get { return _showSampleFlyoutCommand; }
        }

        public IFlyoutManager Flyouts {
            get { return _flyoutManager; }
        }

        private async Task OnShowSampleDialogAsync() {
            var text = await _dialogManager.ShowDialogAsync(new SampleDialogViewModel());
            if (text != null) {
                await _dialogManager.ShowMessageBox(Title, "You entered: " + text);
            }
        }
 
        private Task OnShowSampleFlyoutAsync() {
            return _flyoutManager.ShowFlyout(new SampleFlyoutViewModel());
        }
    }
}