using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ConciseDesign.WPF.CustomControls
{
    public class ColorTip : Control
    {
        static ColorTip()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ColorTip), new FrameworkPropertyMetadata(typeof(ColorTip)));
        }

        
        public Brush TipBrush
        {
            get { return (Brush)GetValue(TipBrushProperty); }
            set { SetValue(TipBrushProperty, value); }
        }

        public static readonly DependencyProperty TipBrushProperty =
            DependencyProperty.Register("TipBrush", typeof(Brush), typeof(ColorTip), new PropertyMetadata(Brushes.Red));

        public double TipRadius
        {
            get { return (double)GetValue(TipRadiusProperty); }
            set { SetValue(TipRadiusProperty, value); }
        }

        public static readonly DependencyProperty TipRadiusProperty =       
            DependencyProperty.Register("TipRadius", typeof(double), typeof(ColorTip), new PropertyMetadata((double)10.00));
        
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(ColorTip), new PropertyMetadata(""));


    }
}
