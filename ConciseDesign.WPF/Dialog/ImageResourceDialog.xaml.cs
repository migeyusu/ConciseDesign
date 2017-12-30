using System.Windows;
using System.Windows.Input;

namespace ConciseDesign.WPF.Dialog
{
    /// <summary>
    /// ImageResourceDialog.xaml 的交互逻辑
    /// </summary>
    public partial class ImageResourceDialog : Window
    {

        public ImageResourceDialog(ImageResourceDialogViewModel viewModel)
        {
            InitializeComponent();
            this.DataContext = viewModel;
        }

        private void Submit(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void ImageResourceDialog_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}

