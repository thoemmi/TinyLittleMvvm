using System.Windows;
using System.Windows.Controls;

namespace TinyLittleMvvm.Wizard.Views {
    /// <summary>
    /// Optional base class for wizard pages.
    /// </summary>
    public class WizardPageView : UserControl {
        static WizardPageView() {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WizardPageView), new FrameworkPropertyMetadata(typeof(WizardPageView)));
        }

        /// <summary>
        /// The <see cref="DependencyProperty"/> for the <see cref="Header"/> property.
        /// </summary>
        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
            "Header", typeof(string), typeof(WizardPageView), new PropertyMetadata(default(string)));

        /// <summary>
        /// Gets or sets the header.
        /// </summary>
        public string Header {
            get { return (string) GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        /// <summary>
        /// The <see cref="DependencyProperty"/> for the <see cref="Subheader"/> property.
        /// </summary>
        public static readonly DependencyProperty SubheaderProperty = DependencyProperty.Register(
            "Subheader", typeof(string), typeof(WizardPageView), new PropertyMetadata(default(string)));

        /// <summary>
        /// The subheader.
        /// </summary>
        public string Subheader {
            get { return (string) GetValue(SubheaderProperty); }
            set { SetValue(SubheaderProperty, value); }
        }
    }
}