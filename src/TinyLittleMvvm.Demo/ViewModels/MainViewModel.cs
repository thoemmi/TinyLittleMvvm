using System.Threading.Tasks;
using System.Windows.Input;

namespace TinyLittleMvvm.Demo.ViewModels {
    public class MainViewModel : PropertyChangedBase, IShell, IOnLoadedHandler {
        private readonly IDialogManager _dialogManager;
        private string _title;

        public MainViewModel(IDialogManager dialogManager, IFlyoutManager flyoutManager, SampleSubViewModel subViewModel) {
            _dialogManager = dialogManager;
            Flyouts = flyoutManager;
            ShowSampleDialogCommand = new AsyncRelayCommand(OnShowSampleDialogAsync);
            ShowSampleFlyoutCommand = new AsyncRelayCommand(OnShowSampleFlyoutAsync);
            SubViewModel = subViewModel;
        }

        public Task OnLoadedAsync() {
            Title = "Tiny Little MVVM Demo";

            SubViewModel.Text = "Hello world";

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

        public ICommand ShowSampleDialogCommand { get; }

        public ICommand ShowSampleFlyoutCommand { get; }

        public SampleSubViewModel SubViewModel { get; }

        public IFlyoutManager Flyouts { get; }

        private async Task OnShowSampleDialogAsync() {
            var text = await _dialogManager.ShowDialogAsync<SampleDialogViewModel, string>();
            if (text != null) {
                await _dialogManager.ShowMessageBox(Title, "You entered: " + text);
            }
        }
 
        private Task OnShowSampleFlyoutAsync() {
            return Flyouts.ShowFlyout<SampleFlyoutViewModel>();
        }
    }
}