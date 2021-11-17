using System.Windows;
using ConciseDesign.WPF.CustomControls;

namespace ConciseDesign.WPF.Windows
{
    public partial class SubmitWindow : Window
    {
        public SubmitWindow()
        {
            this.DataContext = this;
            InitializeComponent();
        }

        public static readonly DependencyProperty DialogContentProperty = DependencyProperty.Register(
            "DialogContent", typeof(IDialogContent), typeof(SubmitWindow),
            new PropertyMetadata(default(IDialogContent)));

        public IDialogContent DialogContent
        {
            get { return (IDialogContent) GetValue(DialogContentProperty); }
            set { SetValue(DialogContentProperty, value); }
        }

        public static readonly DependencyProperty ContentViewProperty = DependencyProperty.Register(
            "ContentView", typeof(object), typeof(SubmitWindow), new PropertyMetadata(default(object)));

        public object ContentView
        {
            get { return (object) GetValue(ContentViewProperty); }
            set { SetValue(ContentViewProperty, value); }
        }

        private void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void SubmitButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (DialogContent != null && !DialogContent.TrySubmit())
            {
                return;
            }

            this.DialogResult = true;
            this.Close();

        }
    }
}