using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using ControlzEx.Theming;

namespace TinyLittleMvvm.Demo.ViewModels {
    public class SampleFlyoutViewModel : DialogViewModel {
        private AccentColorMenuData _currentAccentColor;

        public SampleFlyoutViewModel() {
            AccentColors = ThemeManager.Current.Themes
                .GroupBy(x => x.ColorScheme)
                .Select(a => new AccentColorMenuData {
                    Name = a.Key,
                    ColorBrush = a.First().ShowcaseBrush
                })
                .ToList();
            var theme = ThemeManager.Current.DetectTheme(Application.Current);
            _currentAccentColor = AccentColors.SingleOrDefault(accent => accent.Name == theme?.ColorScheme) ?? AccentColors.First();

            OkCommand = new RelayCommand(OnOk, () => !HasErrors);
            CancelCommand = new RelayCommand(Close);
        }

        public List<AccentColorMenuData> AccentColors { get; }

        public AccentColorMenuData CurrentAccentColor {
            get { return _currentAccentColor; }
            set {
                if (_currentAccentColor != value) {
                    _currentAccentColor = value;
                    ThemeManager.Current.ChangeThemeColorScheme(Application.Current, value.Name);
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
        public string Name { get; set; } = null!;
        public Brush ColorBrush { get; set; } = null!;
    }
}