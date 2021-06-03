using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using ConciseDesign.WPF.Command;
using ConciseDesign.WPF.UserControls;

namespace ConciseDesign.WPF.CustomControls
{
    [TemplatePart(Name = "PART_SubmitButton", Type = typeof(Button))]
    public class ContentDialog : Control
    {
        public const string SubmitButtonName = "PART_SubmitButton";

        static ContentDialog()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ContentDialog),
                new FrameworkPropertyMetadata(typeof(ContentDialog)));
        }

        public static readonly DependencyProperty HeaderHeightProperty = DependencyProperty.Register(
            "HeaderHeight", typeof(double), typeof(ContentDialog), new PropertyMetadata(80d));

        public double HeaderHeight
        {
            get { return (double) GetValue(HeaderHeightProperty); }
            set { SetValue(HeaderHeightProperty, value); }
        }
        
        public string HeaderString
        {
            get { return (string) GetValue(HeaderStringProperty); }
            set { SetValue(HeaderStringProperty, value); }
        }

        public static readonly DependencyProperty HeaderStringProperty =
            DependencyProperty.Register("HeaderString", typeof(string), typeof(ContentDialog),
                new PropertyMetadata(null));

        public FrameworkElement Content
        {
            get { return (FrameworkElement) GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        public static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register("Content", typeof(FrameworkElement), typeof(ContentDialog),
                new PropertyMetadata(null));

        /// <summary>
        /// 是否显示取消按钮
        /// </summary>
        public bool CanCancel
        {
            get { return (bool) GetValue(CanCancelProperty); }
            set { SetValue(CanCancelProperty, value); }
        }

        public static readonly DependencyProperty CanCancelProperty =
            DependencyProperty.Register("CanCancel", typeof(bool), typeof(ContentDialog), new PropertyMetadata(false));

        public static readonly DependencyProperty DialogContentModelProperty = DependencyProperty.Register(
            "DialogContentModel", typeof(IDialogContent), typeof(ContentDialog),
            new PropertyMetadata(default(IDialogContent)));

        public IDialogContent DialogContentModel
        {
            get { return (IDialogContent) GetValue(DialogContentModelProperty); }
            set { SetValue(DialogContentModelProperty, value); }
        }

        /// <summary>
        /// 是否显示submit按钮
        /// </summary>
        public bool CanSubmit
        {
            get { return (bool) GetValue(CanSubmitProperty); }
            set { SetValue(CanSubmitProperty, value); }
        }

        public static readonly DependencyProperty CanSubmitProperty =
            DependencyProperty.Register("CanSubmit", typeof(bool), typeof(ContentDialog), new PropertyMetadata(true));

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            var submitButton = GetTemplateChild(SubmitButtonName) as Button;
            submitButton.Click += SubmitButtonOnClick;
        }

        private void SubmitButtonOnClick(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            if (DialogContentModel != null)
            {
                if (!DialogContentModel.TrySubmit())
                {
                    return;
                }
            }

            var button = sender as Button;
            button.SubmitDialog();
        }
    }
}