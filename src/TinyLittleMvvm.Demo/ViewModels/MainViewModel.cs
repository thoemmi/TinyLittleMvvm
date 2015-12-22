using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

using MahApps.Metro.Controls.Dialogs;

namespace TinyLittleMvvm.Demo.ViewModels {
    public class MainViewModel : PropertyChangedBase, IShell, IOnLoadedHandler, ICancelableOnClosingHandler {
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