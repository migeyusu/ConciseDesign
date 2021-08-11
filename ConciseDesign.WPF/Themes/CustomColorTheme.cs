using System.Runtime.CompilerServices;
using System.Windows.Media;
using System.Windows;
using MaterialDesignColors.ColorManipulation;

namespace ConciseDesign.WPF.Themes
{
    /// <summary>
    /// learn from materialdesignthems https://github.com/MaterialDesignInXAML
    /// </summary>
    public class CustomColorTheme : ResourceDictionary
    {
        private Color? _conciseColor;

        public Color? ConciseColor
        {
            get => _conciseColor;
            set
            {
                if (_conciseColor != value)
                {
                    _conciseColor = value;
                    if (value.HasValue)
                    {
                        SetThemeColor(value.Value);
                    }
                }
            }
        }

        private void SetThemeColor(Color color, [CallerMemberName] string propertyName = null)
        {
            var lighten = color.Lighten();
            var darken = color.Darken();
            SetColor($"{propertyName}Light", lighten);
            SetColor($"{propertyName}Dark", darken);
            SetColor(propertyName, color);
        }

        public void SetColor(string name, Color color)
        {
            var solidColorBrush = new SolidColorBrush(color);
            this[name] = solidColorBrush;
        }
    }
}