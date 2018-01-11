using System;
using System.Windows.Input;

namespace TinyLittleMvvm.Demo.ViewModels {
    public class SampleDialogViewModel : DialogViewModel<string> {
        private string _text;

        public SampleDialogViewModel() {
            OkCommand = new RelayCommand(OnOk, CanOk);
            CancelCommand = new RelayCommand(OnCancel);
            _text = String.Empty;

            AddValidationRule(() => Text, text => !String.IsNullOrEmpty(text), "Text must not be empty");
            ValidateAllRules();
        }

        public string Text {
            get { return _text; }
            set {
                if (Set(ref _text, value)) {
                    ValidateAllRules();
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