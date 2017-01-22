using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TinyLittleMvvm.Wizard.ViewModels {
    /// <summary>
    /// View model for wizards.
    /// </summary>
    public class WizardViewModel : PropertyChangedBase, IOnLoadedHandler {
        private WizardPageViewModel _currentPage;

        /// <summary>
        /// Creates a new <see cref="WizardViewModel"/> object.
        /// </summary>
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

        public List<WizardPageViewModel> PageViewModels { get; internal set; }

        /// <summary>
        /// The view model of the current page.
        /// </summary>
        public WizardPageViewModel CurrentPage {
            get { return _currentPage; }
            private set {
                if (_currentPage != value) {
                    if (_currentPage != null) {
                        UnregisterPageEvents(_currentPage);
                    }

                    _currentPage = value;

                    RegisterPageEvents(_currentPage);

                    NotifyOfPropertyChange();
                }
            }
        }

        private void UnregisterPageEvents(WizardPageViewModel pageViewModel) {
            pageViewModel.InternalGoNext -= PageViewModelOnInternalGoNext;
            pageViewModel.InternalGoBack -= PageViewModelOnInternalGoBack;
        }

        private void RegisterPageEvents(WizardPageViewModel pageViewModel) {
            pageViewModel.InternalGoNext += PageViewModelOnInternalGoNext;
            pageViewModel.InternalGoBack += PageViewModelOnInternalGoBack;
        }

        private void PageViewModelOnInternalGoBack(object sender, EventArgs eventArgs) {
            BackCommand.Execute(null);
        }

        private void PageViewModelOnInternalGoNext(object sender, EventArgs eventArgs) {
            NextCommand.Execute(null);
        }

        private bool CanBack() {
            if (PageViewModels?.FirstOrDefault() == _currentPage) {
                return false;
            }
            return _currentPage?.CanGoBack ?? false;
        }

        private async Task OnBackAsync() {
            var currentPos = PageViewModels.IndexOf(_currentPage);
            if (currentPos > 0) {
                CurrentPage = PageViewModels[currentPos - 1];
                await CurrentPage.OnEnterBackwardAsync();
            }
        }

        private bool CanNext() {
            if (PageViewModels?.LastOrDefault() == _currentPage) {
                return false;
            }
            return _currentPage?.CanGoNext ?? false;
        }

        private async Task OnNextAsync() {
            var currentPos = PageViewModels.IndexOf(_currentPage);
            if (currentPos < PageViewModels.Count - 1) {
                CurrentPage = PageViewModels[currentPos + 1];
                await CurrentPage.OnEnterForwardAsync();
            }
        }

        private bool CanCancel() {
            return _currentPage?.CanCancel ?? true;
        }

        private Task OnCancelAsync() {
            Closed?.Invoke(this, EventArgs.Empty);
            return Task.FromResult(0);
        }

        private bool CanFinish() {
            return _currentPage?.CanFinish ?? _currentPage == PageViewModels.Last();
        }

        public bool IsCancelVisible => _currentPage?.IsCancelVisible ?? _currentPage != PageViewModels.Last();
        public bool IsFinishVisible => _currentPage?.IsFinishVisible ?? _currentPage == PageViewModels.Last();


        private Task OnFinishAsync() {
            Closed?.Invoke(this, EventArgs.Empty);
            return Task.FromResult(0);
        }

        public Task OnLoadedAsync() {
            CurrentPage = PageViewModels.First();
            return CurrentPage.OnEnterForwardAsync();
        }

        internal event EventHandler Closed;
    }
}