using System.Windows;
using System.Windows.Controls;

namespace ConciseDesign.WPF.CustomControls
{
    public class SelectBox : ComboBox
    {


        public string HeaderText
        {
            get { return (string)GetValue(HeaderTextProperty); }
            set { SetValue(HeaderTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HeaderText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeaderTextProperty =
            DependencyProperty.Register("HeaderText", typeof(string), typeof(SelectBox), new PropertyMetadata("选择一个项"));



        static SelectBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SelectBox), new FrameworkPropertyMetadata(typeof(SelectBox)));
        }
    }
}
