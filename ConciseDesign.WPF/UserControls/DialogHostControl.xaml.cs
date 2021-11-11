using System;
using System.Collections.Concurrent;
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
        
        private ConcurrentQueue<DialogEntry> _dialogEntriesQueue = new ConcurrentQueue<DialogEntry>();

        /// <summary>
        /// raise message as dialog content
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="dialogId">trace dialog status</param>
        /// <returns></returns>
        public async Task RaiseMessageAsync(string msg, Guid dialogId = default)
        {
            await RaiseDialogAsync(new MessageDialog() {DataContext = msg}, dialogId);
        }

        /// <summary>
        /// dialog with acceptance
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="dialogId">trace dialog status</param>
        /// <returns></returns>
        public async Task<bool> RaiseSubmitAsync(string msg, Guid dialogId = default)
        {
            return await RaiseDialogAsync(new ConfirmDialog() {Description = msg}, dialogId);
        }

        public Task<bool> RaiseDialogAsync(object content, Guid dialogId = default)
        {
            var dialogEntry = new DialogEntry()
            {
                DialogContent = content,
                Id = dialogId,
            };
            _dialogEntriesQueue.Enqueue(dialogEntry);
            LoopDialog();
            return dialogEntry.TaskCompletionSource.Task;
        }

        public bool Contains(Guid dialogId)
        {
            return _dialogEntriesQueue.Any(entry => entry.Id == dialogId);
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
            
            _dialogEntriesQueue.TryDequeue(out var dialogEntry);
            dialogEntry.TaskCompletionSource.SetResult(dialogResult);
            IsDialogOpened = false;
            LoopDialog();
        }

        private void LoopDialog()
        {
            if (!IsDialogOpened &&_dialogEntriesQueue.TryPeek(out var dialogEntry))
            {
                IsDialogOpened = true;
                this.DialogContent = dialogEntry.DialogContent;
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

            public Guid Id { get; set; }

            public DialogEntry()
            {
                this.TaskCompletionSource = new TaskCompletionSource<bool>();
            }
        }
    }
}