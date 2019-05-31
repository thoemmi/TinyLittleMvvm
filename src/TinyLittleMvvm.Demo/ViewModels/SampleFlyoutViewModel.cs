using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using MahApps.Metro;

namespace TinyLittleMvvm.Demo.ViewModels {
    public class SampleFlyoutViewModel : DialogViewModel {
        private AccentColorMenuData _currentAccentColor;

        public SampleFlyoutViewModel() {
            AccentColors = ThemeManager.ColorSchemes
                .Select(a => new AccentColorMenuData {
                    Name = a.Name,
                    ColorBrush = a.ShowcaseBrush
                })
                .ToList();
            var theme = ThemeManager.DetectTheme(Application.Current);
            _currentAccentColor = AccentColors.Single(accent => accent.Name == theme.ColorScheme);

            OkCommand = new RelayCommand(OnOk, () => !HasErrors);
            CancelCommand = new RelayCommand(Close);
        }

        public List<AccentColorMenuData> AccentColors { get; }

        public AccentColorMenuData CurrentAccentColor {
            get { return _currentAccentColor; }
            set {
                if (_currentAccentColor != value) {
                    _currentAccentColor = value;
                    ThemeManager.ChangeThemeColorScheme(Application.Current, value.Name);
                }
            }
        }

        public ICommand OkCommand { get; }

        public ICommand CancelCommand { get; }

        private void OnOk() {
            Close();
        }
    }

    public class AccentColorMenuData {
        public string Name { get; set; }
        public Brush ColorBrush { get; set; }
    }
}