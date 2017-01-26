using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

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

        /// <summary>
        /// The <see cref="DependencyProperty"/> for the <see cref="Icon"/> property.
        /// </summary>
        public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
            "Icon", typeof(ImageSource), typeof(WizardPageView), new PropertyMetadata(default(ImageSource)));

        /// <summary>
        ///  The icon to be displayed in the header.
        /// </summary>
        public ImageSource Icon {
            get { return (ImageSource) GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }
    }
}