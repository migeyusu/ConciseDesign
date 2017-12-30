using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ConciseDesign.WPF.CustomControls
{
    public class ActiveBackButton : Button
    {
        public Brush MouseOverBackground
        {
            get { return (Brush)GetValue(MouseOverBackgroundProperty); }
            set { SetValue(MouseOverBackgroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MouseOverBackground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MouseOverBackgroundProperty =
            DependencyProperty.Register("MouseOverBackground", typeof(Brush), typeof(ActiveBackButton), new PropertyMetadata());


        public Brush PressBackground
        {
            get { return (Brush)GetValue(PressBackgroundProperty); }
            set { SetValue(PressBackgroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PressBackground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PressBackgroundProperty =
            DependencyProperty.Register("PressBackground", typeof(Brush), typeof(ActiveBackButton), new PropertyMetadata(new SolidColorBrush()));



        static ActiveBackButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ActiveBackButton), new FrameworkPropertyMetadata(typeof(ActiveBackButton)));
        }
    }
}
