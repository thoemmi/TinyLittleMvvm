using TinyLittleMvvm.Wizard.ViewModels;

namespace TinyLittleMvvm.Demo.WizardDemo.ViewModels {
    public class Page1ViewModel : WizardPageViewModel {
        private string _userName;
        private string _password;

        public string UserName {
            get { return _userName; }
            set {
                if (_userName != value) {
                    _userName = value;
                    NotifyOfPropertyChange(nameof(UserName));
                }
            }
        }

        public string Password {
            get { return _password; }
            set {
                if (_password != value) {
                    _password = value;
                    NotifyOfPropertyChange(nameof(Password));
                }
            }
        }

        public override bool CanGoNext => UserName == "Admin" && Password == "Admin";
    }
}