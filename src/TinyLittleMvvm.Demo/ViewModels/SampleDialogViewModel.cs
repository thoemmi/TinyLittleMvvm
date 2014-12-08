using System;
using System.Windows.Input;

namespace TinyLittleMvvm.Demo.ViewModels {
    public class SampleDialogViewModel : DialogViewModel<string> {
        private readonly ICommand _okCommand;
        private readonly ICommand _cancelCommand;
        private string _text;

        public SampleDialogViewModel() {
            _okCommand = new RelayCommand(OnOk, CanOk);
            _cancelCommand = new RelayCommand(OnCancel);
            _text = String.Empty;

            AddValidationRule(() => Text, text => !String.IsNullOrEmpty(text), "Text must not be empty");
        }

        public string Text {
            get { return _text; }
            set {
                if (_text != value) {
                    _text = value;
                    NotifyOfPropertyChange(() => Text);
                }
            }
        }

        public ICommand OkCommand {
            get { return _okCommand; }
        }

        public ICommand CancelCommand {
            get { return _cancelCommand; }
        }

        private bool CanOk() {
            return ValidateAllRules();
        }

        private void OnOk() {
            Close(Text);
        }

        private void OnCancel() {
            Close(null);
        }
    }
}