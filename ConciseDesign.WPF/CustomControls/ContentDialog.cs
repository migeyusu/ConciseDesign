using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ConciseDesign.WPF.UserControls;

namespace ConciseDesign.WPF.CustomControls
{
    /// <summary>
    /// used as dialog content wrapper of <see cref="DialogHostControl"/>.
    /// set  content and header to create a dialog which user can submit or cancel
    /// <para>submit button can auto take use of <see cref="IDialogContent"/></para>
    /// </summary>
    [TemplatePart(Name = "PART_SubmitButton", Type = typeof(Button))]
    public class ContentDialog : HeaderedContentControl
    {
        public const string SubmitButtonName = "PART_SubmitButton";

        static ContentDialog()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ContentDialog),
                new FrameworkPropertyMetadata(typeof(ContentDialog)));
            VerticalContentAlignmentProperty.OverrideMetadata(typeof(ContentDialog),
                new FrameworkPropertyMetadata(System.Windows.VerticalAlignment.Center));
            HorizontalContentAlignmentProperty.OverrideMetadata(typeof(ContentDialog),
                new FrameworkPropertyMetadata(System.Windows.HorizontalAlignment.Center));
        }

        public static readonly DependencyProperty HeaderHeightProperty = DependencyProperty.Register(
            "HeaderHeight", typeof(double), typeof(ContentDialog), new PropertyMetadata(80d));

        [Obsolete("Use header instead")]
        public double HeaderHeight
        {
            get { return (double) GetValue(HeaderHeightProperty); }
            set { SetValue(HeaderHeightProperty, value); }
        }

        [Obsolete("Use header instead")]
        public string HeaderString
        {
            get { return (string) GetValue(HeaderStringProperty); }
            set { SetValue(HeaderStringProperty, value); }
        }

        public static readonly DependencyProperty HeaderStringProperty =
            DependencyProperty.Register("HeaderString", typeof(string), typeof(ContentDialog),
                new PropertyMetadata(null));

        public static readonly DependencyProperty HeaderBackgroundProperty = DependencyProperty.Register(
            "HeaderBackground", typeof(Brush), typeof(ContentDialog), new PropertyMetadata(Brushes.DodgerBlue));

        [Obsolete("Use header instead")]
        public Brush HeaderBackground
        {
            get { return (Brush) GetValue(HeaderBackgroundProperty); }
            set { SetValue(HeaderBackgroundProperty, value); }
        }

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

        [Obsolete("Submit will be called on Content property")]
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
            if (Content is IDialogContent dialogContent)
            {
                if (!dialogContent.TrySubmit())
                {
                    return;
                }
            }

            var button = sender as Button;
            button.SubmitDialog();
        }
    }
}