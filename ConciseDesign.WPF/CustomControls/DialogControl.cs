using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ConciseDesign.WPF.CustomControls
{
    
    /// <summary>
    /// a simple dialog 
    /// </summary>
    public class DialogControl: ContentControl
    {
        static DialogControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DialogControl),
                new FrameworkPropertyMetadata(typeof(DialogControl)));
            VerticalContentAlignmentProperty.OverrideMetadata(typeof(DialogControl),new FrameworkPropertyMetadata(VerticalAlignment.Center));
            HorizontalContentAlignmentProperty.OverrideMetadata(typeof(DialogControl),new FrameworkPropertyMetadata(HorizontalAlignment.Center));
            BackgroundProperty.OverrideMetadata(typeof(DialogControl),new FrameworkPropertyMetadata(Brushes.Transparent));
            PaddingProperty.OverrideMetadata(typeof(DialogControl),new FrameworkPropertyMetadata(new Thickness(10)));
        }

        public static readonly DependencyProperty IsDialogOpenProperty = DependencyProperty.Register(
            "IsDialogOpen", typeof(bool), typeof(DialogControl), new PropertyMetadata(default(bool)));

        /// <summary>
        /// responsible for dialog status
        /// </summary>
        public bool IsDialogOpen
        {
            get { return (bool) GetValue(IsDialogOpenProperty); }
            set { SetValue(IsDialogOpenProperty, value); }
        }
    }
}