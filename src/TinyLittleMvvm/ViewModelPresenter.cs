using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace TinyLittleMvvm {
    /// <summary>
    /// A content control presenting a view for a given view model via binding.
    /// </summary>
    public class ViewModelPresenter : ContentControl {
        static ViewModelPresenter() {
            FocusableProperty.OverrideMetadata(typeof(ViewModelPresenter), new FrameworkPropertyMetadata(false));
        }

        /// <summary>
        /// The dependency property for the bindable view model.
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(object), typeof(ViewModelPresenter),
                new PropertyMetadata(default(object), OnViewModelChanged));

        /// <summary>
        /// The view model for which this control should display the corresponding view.
        /// </summary>
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