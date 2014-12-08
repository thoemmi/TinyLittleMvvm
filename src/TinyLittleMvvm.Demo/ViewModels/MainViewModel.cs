using System.Threading.Tasks;

namespace TinyLittleMvvm.Demo.ViewModels {
    public class MainViewModel : PropertyChangedBase, IShell, IOnLoadedHandler {
        private string _title;

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
    }
}