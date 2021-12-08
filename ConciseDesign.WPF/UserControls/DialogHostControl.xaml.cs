using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ConciseDesign.WPF.CustomControls;

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

        public static readonly DependencyProperty LastCommandParameterProperty = DependencyProperty.Register(
            "LastCommandParameter", typeof(object), typeof(DialogHostControl), new PropertyMetadata(default(object)));

        public object LastCommandParameter
        {
            get { return (object) GetValue(LastCommandParameterProperty); }
            set { SetValue(LastCommandParameterProperty, value); }
        }

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

        public static readonly DependencyProperty CurrentDialogProperty = DependencyProperty.Register(
            "CurrentDialog", typeof(DialogEntry), typeof(DialogHostControl),
            new PropertyMetadata(default(DialogEntry)));

        public DialogEntry CurrentDialog
        {
            get { return (DialogEntry) GetValue(CurrentDialogProperty); }
            set { SetValue(CurrentDialogProperty, value); }
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

        public IReadOnlyList<DialogEntry> DialogEntries =>
            new ReadOnlyCollection<DialogEntry>(_dialogEntriesQueue.ToArray());

        /// <summary>
        /// dialog waiting queue, dialog will 
        /// </summary>
        private readonly ConcurrentQueue<DialogEntry> _dialogEntriesQueue = new ConcurrentQueue<DialogEntry>();

        /// <summary>
        /// raise message as dialog content
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="dialogId">trace dialog status</param>
        /// <returns></returns>
        public DialogEntry RaiseMessageAsync(string msg, Guid dialogId = default)
        {
            return RaiseDialogAsync(new MessageDialog() {DataContext = msg}, dialogId);
        }

        public DialogEntry RaiseSubmitAsync(string title, object content, Guid dialogId = default)
        {
            return RaiseDialogAsync(
                new ContentDialog() {Header = title, Content = content, CanCancel = true, CanSubmit = true}, dialogId);
        }

        /// <summary>
        /// dialog with acceptance
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="dialogId">trace dialog status</param>
        /// <returns></returns>
        public DialogEntry RaiseSubmitAsync(string msg, Guid dialogId = default)
        {
            return RaiseDialogAsync(new ConfirmDialog() {Description = msg}, dialogId);
        }

        /// <summary>
        /// raise view for a dialog content
        /// </summary>
        /// <param name="content"></param>
        /// <param name="dialogId"></param>
        /// <returns></returns>
        public DialogEntry RaiseDialogAsync(object content, Guid dialogId = default)
        {
            var dialogEntry = new DialogEntry(content, dialogId);
            _dialogEntriesQueue.Enqueue(dialogEntry);
            TryRaiseDialog();
            return dialogEntry;
        }

        /// <summary>
        /// check if exist in queue
        /// </summary>
        /// <param name="dialogId"></param>
        /// <returns></returns>
        public bool Contains(Guid dialogId)
        {
            if (CurrentDialog?.Id.Equals(dialogId) == true)
            {
                return true;
            }

            return _dialogEntriesQueue.Any(entry => entry.Id == dialogId);
        }

        /// <summary>
        /// check if exist in queue
        /// </summary>
        /// <param name="dialogEntry"></param>
        /// <returns></returns>
        public bool Contains(DialogEntry dialogEntry)
        {
            if (Equals(CurrentDialog, dialogEntry))
            {
                return true;
            }

            return _dialogEntriesQueue.Any(entry => entry.Equals(dialogEntry));
        }

        /// <summary>
        /// cancel directly without any outcome 
        /// </summary>
        private void Close(ExecutedRoutedEventArgs e, bool dialogResult = true)
        {
            if (!IsDialogOpened)
            {
                return;
            }

            LastCommandParameter = e.Parameter;
            e.Handled = true; //防止关闭下一级的dialog
            this.CurrentDialog.TaskCompletionSource.TrySetResult(dialogResult);
            TryRaiseDialog();
        }

        private async void TryRaiseDialog()
        {
            while (!IsDialogOpened && _dialogEntriesQueue.TryDequeue(out var dialogEntry)
                                   && !dialogEntry.TaskCompletionSource.Task.IsCanceled)
            {
                // LastCommandParameter = null;
                IsDialogOpened = true;
                this.CurrentDialog = dialogEntry;
                this.DialogContent = dialogEntry.DialogContent;
                VisualStateManager.GoToState(this, OpenDialogState, true);
                await dialogEntry;
                VisualStateManager.GoToState(this, CloseDialogState, true);
                DialogContent = null;
                IsDialogOpened = false;
                CurrentDialog = default;
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
    }

    /// <summary>
    /// 对话项
    /// </summary>
    public class DialogEntry
    {
        internal TaskCompletionSource<bool> TaskCompletionSource { get; }

        public object DialogContent { get; }

        public Guid Id { get; }

        internal DialogEntry(object dialogContent, Guid id)
        {
            DialogContent = dialogContent;
            Id = id;
            this.TaskCompletionSource = new TaskCompletionSource<bool>();
        }

        public void Cancel()
        {
            this.TaskCompletionSource.TrySetCanceled();
        }

        public void Close()
        {
            this.TaskCompletionSource.TrySetResult(false);
        }


        protected bool Equals(DialogEntry other)
        {
            return Equals(DialogContent, other.DialogContent) && Id.Equals(other.Id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((DialogEntry) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((DialogContent != null ? DialogContent.GetHashCode() : 0) * 397) ^ Id.GetHashCode();
            }
        }
    }
}