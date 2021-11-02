using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ConciseDesign.WPF.UserControls
{
    /* dialog调用分为两种形式：
     * 1.原始形式：返回command execute的callback
     * 2.高级形式：返回true/false 并返回execute的parameter作为 out result
     * 要么使用无返回的callback，要么使用返回的out result，同时使用返回值和callback不妥 */

    /// <summary>
    /// DialogHostControl.xaml 的交互逻辑
    /// </summary>
    public partial class DialogHostControl : UserControl
    {
        public const string OpenDialogState = "OpenDialog";

        public const string CloseDialogState = "CloseDialog";

        public static readonly DependencyProperty IsDialogOpenedProperty = DependencyProperty.Register(
            "IsDialogOpened", typeof(bool), typeof(DialogHostControl), new PropertyMetadata(default(bool)));

        /// <summary>
        /// 指示当前打开状态
        /// </summary>
        public bool IsDialogOpened
        {
            get { return (bool) GetValue(IsDialogOpenedProperty); }
            set { SetValue(IsDialogOpenedProperty, value); }
        }

        /// <summary>
        /// 当前对话的内容
        /// </summary>
        public object DialogContent
        {
            get { return (object) GetValue(DialogContentProperty); }
            private set { SetValue(DialogContentProperty, value); }
        }

        public static readonly DependencyProperty DialogContentProperty =
            DependencyProperty.Register("DialogContent", typeof(object), typeof(DialogHostControl),
                new PropertyMetadata(default(object)));

        // private readonly AutoResetEvent _autoResetEvent = new AutoResetEvent(false);

        /// <summary>
        /// 由于命令总是从ui thread发出，不需要线程安全处理
        /// </summary>
        private Queue<DialogEntry> _dialogEntries = new Queue<DialogEntry>();

        // private bool _dialogResult;

        public async Task<bool> RaiseMessageAsync(string msg)
        {
            return await RaiseDialogAsync(new MessageDialog() {DataContext = msg});
        }

        public async Task<bool> RaiseSubmitAsync(string msg)
        {
            return await RaiseDialogAsync(new ConfirmDialog() {Description = msg});
        }

        public Task<bool> RaiseDialogAsync(object content)
        {
            var dialogEntry = new DialogEntry()
            {
                DialogContent = content,
            };
            _dialogEntries.Enqueue(dialogEntry);
            LoopDialog();
            return dialogEntry.TaskCompletionSource.Task;
            /*var task = Task.Run(() =>
                {
                    _dialogResult = false;
                    _autoResetEvent.WaitOne();
                    return _dialogResult;
                });*/
        }

        /// <summary>
        /// cancel directly without any outcome 
        /// </summary>
        private void Close(ExecutedRoutedEventArgs e, bool dialogResult = true)
        {
            e.Handled = true; //防止关闭下一级的dialog
            // _dialogResult = dialogResult;
            VisualStateManager.GoToState(this, CloseDialogState, true);
            DialogContent = null;
            var dialogEntry = _dialogEntries.Dequeue();
            dialogEntry.TaskCompletionSource.SetResult(dialogResult);
            IsDialogOpened = false;
            LoopDialog();
        }

        private void LoopDialog()
        {
            if (!IsDialogOpened && _dialogEntries.Any())
            {
                IsDialogOpened = true;
                this.DialogContent = _dialogEntries.Peek().DialogContent;
                VisualStateManager.GoToState(this, OpenDialogState, true);
            }
        }


        public DialogHostControl()
        {
            this.LoadViewFromUri("/ConciseDesign.WPF;component/UserControls/DialogHostControl.xaml");
            // InitializeComponent();
        }

        private void OpenDialog_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, OpenDialogState, true);
        }

        private void CloseDialog_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Close(e);
        }

        private void CancelDialog_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Close(e, false);
        }

        private void SubmitDialog_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Close(e);
        }

        /// <summary>
        /// 对话项
        /// </summary>
        internal class DialogEntry
        {
            public TaskCompletionSource<bool> TaskCompletionSource { get; }

            public object DialogContent { get; set; }

            public DialogEntry()
            {
                this.TaskCompletionSource = new TaskCompletionSource<bool>();
            }
        }
    }
}