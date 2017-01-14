using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

using MahApps.Metro.Controls.Dialogs;

namespace TinyLittleMvvm.Demo.ViewModels {
    public class MainViewModel : PropertyChangedBase, IShell, IOnLoadedHandler, ICancelableOnClosingHandler {
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

        public bool OnClosing() {
            var mySettings = new MetroDialogSettings()
            {
                AffirmativeButtonText = "Quit",
                NegativeButtonText = "Cancel",
                AnimateShow = true,
                AnimateHide = false
            };

            _dialogManager.ShowMessageBox("Quit application?",
                "Sure you want to quit application?",
                MessageDialogStyle.AffirmativeAndNegative, mySettings)
                .ContinueWith(t => {
                    if (t.Result == MessageDialogResult.Affirmative)
                    {
                        Application.Current.Shutdown();
                    }
                }, TaskScheduler.FromCurrentSynchronizationContext());

            return true;
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