using System.Windows;
using System.Windows.Controls;

namespace ConciseDesign.WPF.UserControls
{
    /// <summary>
    /// ConfirmDialog.xaml 的交互逻辑
    /// </summary>
    public partial class ConfirmDialog : UserControl
    {

        public string Return
        {
            get { return (string)GetValue(ReturnProperty); }
            set { SetValue(ReturnProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Return.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ReturnProperty =
            DependencyProperty.Register("Return", typeof(string), typeof(ConfirmDialog), new PropertyMetadata(""));



        public string Description
        {
            get { return (string)GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Description.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DescriptionProperty =
            DependencyProperty.Register("Description", typeof(string), typeof(ConfirmDialog), new PropertyMetadata(""));



        public ConfirmDialog()
        {
            InitializeComponent();
        }
    }
}
