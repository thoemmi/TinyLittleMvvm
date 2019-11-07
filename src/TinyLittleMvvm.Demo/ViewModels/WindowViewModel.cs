using System.Threading.Tasks;

namespace TinyLittleMvvm.Demo.ViewModels
{
    public class WindowViewModel : PropertyChangedBase, IOnLoadedHandler
    {
        private string _text;

        public string Text
        {
            get => _text;
            set
            {
                if (_text != value)
                {
                    _text = value;
                    NotifyOfPropertyChange(() => Text);
                }
            }
        }

        public Task OnLoadedAsync()
        {
            Text = "A window Opened as a dialog";
            return Task.CompletedTask;
        }
    }
}
