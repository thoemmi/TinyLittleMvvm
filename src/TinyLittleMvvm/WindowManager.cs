using System.Windows;

namespace TinyLittleMvvm {
    public class WindowManager {
        public Window ShowWindow<TViewModel>() {
            var view = (Window)ViewLocator.GetViewForViewModel<TViewModel>();
            view.Show();
            return view;
        }
    }
}