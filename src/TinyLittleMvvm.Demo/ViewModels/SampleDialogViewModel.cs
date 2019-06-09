using System;
using System.Windows.Input;

namespace TinyLittleMvvm.Demo.ViewModels {
    public class SampleDialogViewModel : DialogViewModel<string> {
        private string _text;

        public SampleDialogViewModel(ScopedService scopedService) {
            OkCommand = new RelayCommand(OnOk, CanOk);
            CancelCommand = new RelayCommand(OnCancel);

            AddValidationRule(() => Text, text => !String.IsNullOrEmpty(text), "Text must not be empty");
        }

        public string Text {
            get { return _text; }
            set {
                if (_text != value) {
                    _text = value;
                    ValidateAllRules();
                    NotifyOfPropertyChange(() => Text);
                }
            }
        }

        public ICommand OkCommand { get; }

        public ICommand CancelCommand { get; }

        private bool CanOk() {
            return !HasErrors;
        }

        private void OnOk() {
            Close(Text);
        }

        private void OnCancel() {
            Close(null);
        }
    }
}