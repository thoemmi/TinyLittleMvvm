using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using MahApps.Metro;

namespace TinyLittleMvvm.Demo.ViewModels {
    public class SampleFlyoutViewModel : DialogViewModel {
        private readonly ICommand _okCommand;
        private readonly ICommand _cancelCommand;
        private readonly List<AccentColorMenuData> _accentColors;
        private AccentColorMenuData _currentAccentColor;

        public SampleFlyoutViewModel() {
            _accentColors = ThemeManager.Accents
                .Select(a => new AccentColorMenuData {
                    Name = a.Name,
                    ColorBrush = a.Resources["AccentColorBrush"] as Brush
                })
                .ToList();
            var theme = ThemeManager.DetectAppStyle(Application.Current);
            _currentAccentColor = _accentColors.Single(accent => accent.Name == theme.Item2.Name);

            _okCommand = new RelayCommand(OnOk, () => !HasErrors);
            _cancelCommand = new RelayCommand(Close);
        }

        public List<AccentColorMenuData> AccentColors {
            get { return _accentColors; }
        }

        public AccentColorMenuData CurrentAccentColor {
            get { return _currentAccentColor; }
            set {
                if (_currentAccentColor != value) {
                    _currentAccentColor = value;
                    var theme = ThemeManager.DetectAppStyle(Application.Current);
                    var accent = ThemeManager.GetAccent(_currentAccentColor.Name);
                    ThemeManager.ChangeAppStyle(Application.Current, accent, theme.Item1);
                }
            }
        }

        public ICommand OkCommand {
            get { return _okCommand; }
        }

        public ICommand CancelCommand {
            get { return _cancelCommand; }
        }

        private void OnOk() {
            Close();
        }
    }

    public class AccentColorMenuData {
        public string Name { get; set; }
        public Brush ColorBrush { get; set; }
    }
}