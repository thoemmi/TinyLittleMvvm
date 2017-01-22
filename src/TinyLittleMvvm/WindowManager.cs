using System;
using System.Linq;
using System.Windows;
using Autofac;
using TinyLittleMvvm.Wizard.ViewModels;

namespace TinyLittleMvvm {
    internal class WindowManager : IWindowManager {
        public Window ShowWindow<TViewModel>(Window owningWindow = null) {
            var window = (Window)ViewLocator.GetViewForViewModel<TViewModel>();
            window.Owner = owningWindow;
            window.Show();
            return window;
        }

        public Window ShowWindow(object viewModel, Window owningWindow = null) {
            var window = (Window)ViewLocator.GetViewForViewModel(viewModel);
            window.Owner = owningWindow;
            window.Show();
            return window;
        }

        public Window ShowWizard<TWizardViewModel>(params WizardPageViewModel[] pageViewModels) where TWizardViewModel : WizardViewModel {
            var viewModel = BootstrapperBase.Container.Resolve<TWizardViewModel>();
            viewModel.PageViewModels = pageViewModels.ToList();

            var window = (Window)ViewLocator.GetViewForViewModel(viewModel);
            window.Resources.MergedDictionaries.Add(new ResourceDictionary {
                Source = new Uri("pack://application:,,,/MahApps.Metro;component/Styles/FlatButton.xaml")
            });

            viewModel.Closed += (sender, args) => window.Close();

            window.Show();
            return window;
        }

        public Window ShowWizard(params WizardPageViewModel[] pageViewModels) {
            return ShowWizard<WizardViewModel>(pageViewModels);
        }
    }
}