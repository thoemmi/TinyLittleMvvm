namespace TinyLittleMvvm.Demo.ViewModels {
    public class SampleSubViewModel : PropertyChangedBase {
        private string _text;

        public string Text {
            get { return _text; }
            set { Set(ref _text, value); }
        }
    }
}