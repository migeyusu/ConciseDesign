using System.Windows;
using System.Windows.Controls;

namespace ConciseDesign.WPF.CustomControls
{

    public class ConciseButton : Button
    {
        static ConciseButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ConciseButton), new FrameworkPropertyMetadata(typeof(ConciseButton)));
        }



        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CornerRadius.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(ConciseButton), new PropertyMetadata(new CornerRadius(0)));

            
    }
}
