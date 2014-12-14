using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace TinyLittleMvvm {
    public class ViewModelPresenter : ContentControl {
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(object), typeof(ViewModelPresenter),
                new PropertyMetadata(default(object), OnViewModelChanged));

        public object ViewModel {
            get { return GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        private static void OnViewModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            if (DesignerProperties.GetIsInDesignMode(d)) {
                return;
            }

            var self = (ViewModelPresenter)d;
            self.Content = null;

            if (e.NewValue != null) {
                var view = ViewLocator.GetViewForViewModel(e.NewValue);
                self.Content = view;
            }
        }
    }
}