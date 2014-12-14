using System.Threading.Tasks;
using System.Windows.Input;

namespace TinyLittleMvvm.Demo.ViewModels {
    public class MainViewModel : PropertyChangedBase, IShell, IOnLoadedHandler {
        private readonly IDialogManager _dialogManager;
        private readonly IFlyoutManager _flyoutManager;
        private readonly ICommand _showSampleDialogCommand;
        private readonly ICommand _showSampleFlyoutCommand;
        private readonly SampleSubViewModel _subViewModel;
        private string _title;

        public MainViewModel(IDialogManager dialogManager, IFlyoutManager flyoutManager, SampleSubViewModel subViewModel) {
            _dialogManager = dialogManager;
            _flyoutManager = flyoutManager;
            _showSampleDialogCommand = new AsyncRelayCommand(OnShowSampleDialogAsync);
            _showSampleFlyoutCommand = new AsyncRelayCommand(OnShowSampleFlyoutAsync);
            _subViewModel = subViewModel;
        }

        public Task OnLoadedAsync() {
            Title = "Tiny Little MVVM Demo";

            _subViewModel.Text = "Hello world";

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

        public SampleSubViewModel SubViewModel {
            get { return _subViewModel; }
        }

        public IFlyoutManager Flyouts {
            get { return _flyoutManager; }
        }

        private async Task OnShowSampleDialogAsync() {
            var text = await _dialogManager.ShowDialogAsync<SampleDialogViewModel, string>();
            if (text != null) {
                await _dialogManager.ShowMessageBox(Title, "You entered: " + text);
            }
        }
 
        private Task OnShowSampleFlyoutAsync() {
            return _flyoutManager.ShowFlyout<SampleFlyoutViewModel>();
        }
    }
}