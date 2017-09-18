using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ConciseDesign.WPF.Controls
{
    /* dialog调用分为两种形式：
     * 1.原始形式：返回command execute的callback
     * 2.高级形式：返回true/false 并返回execute的parameter作为 out result
     * 要么使用无返回的callback，要么使用返回的out result，同时使用返回值和callback不妥
     */

    /// <summary>
    /// DialogHostControl.xaml 的交互逻辑
    /// </summary>
    public partial class DialogHostControl : UserControl
    {
        public const string OpenDialogState = "OpenDialog";

        public const string CloseDialogState = "CloseDialog";
        
        public object DialogContent
        {
            get { return (object)GetValue(DialogContentProperty); }
            set { SetValue(DialogContentProperty, value); }
        }

        public static readonly DependencyProperty DialogContentProperty =
            DependencyProperty.Register("DialogContent", typeof(object), typeof(DialogHostControl), new PropertyMetadata(default(object)));

        private ExecutedRoutedEventHandler _executedRoutedEventHandler;

        //第一类base dialog
        public void RaiseDialog(object content, ExecutedRoutedEventHandler handler)
        {
            DialogContent = content;
            _executedRoutedEventHandler = handler;
            VisualStateManager.GoToState(this, OpenDialogState, true);
        }

        private  AutoResetEvent _autoResetEvent = new AutoResetEvent(false);

        private bool _dialogResult;

        private object _dialogTransferObject = null;

        public Task<bool> RaiseDialogAsync(object content)
        {
            _autoResetEvent=new AutoResetEvent(false);
            DialogContent = content;
#if past
            VisualStateManager.GoToState(this, OpenDialogState, true);
            return Task.Run(() =>
            {
                _autoResetEvent.WaitOne();
                return _dialogResult;
            });
#else
            var task = Task.Run(() => {
                _dialogResult = false;
                _autoResetEvent.WaitOne();
                return _dialogResult;
            });
            VisualStateManager.GoToState(this, OpenDialogState, true);
            return task;
#endif
        }

        public async Task<bool> RaiseMessageAsync(string msg)
        {
            return await RaiseDialogAsync(new MessageDialog() {DataContext = msg});
        }

        public async Task<bool> RaiseSubmitTaskAsync(string msg)
        {
            return await RaiseDialogAsync(new ConfirmDialog() {Description = msg});
        }

        public DialogHostControl()
        {
            InitializeComponent();
        }

        private void OpenDialog_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, OpenDialogState, true);
        }

        private void CloseDialog_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, CloseDialogState, true);
            _executedRoutedEventHandler?.Invoke(sender, e);
            _executedRoutedEventHandler = null;
            DialogContent = null;
        }

        private void CancelDialog_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, CloseDialogState, true);
            //_executedRoutedEventHandler?.Invoke(sender, e);
            //_executedRoutedEventHandler = null;
            _dialogTransferObject = e.Parameter;
            _dialogResult = false;
            _autoResetEvent.Set();
            _autoResetEvent.Dispose(); 
            DialogContent = null;
            
        }

        private void SubmitDialog_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, CloseDialogState, true);
           
            //_executedRoutedEventHandler?.Invoke(sender, e);
            //_executedRoutedEventHandler = null;
            _dialogTransferObject = e.Parameter;
            _dialogResult = true;
            _autoResetEvent.Set();
            _autoResetEvent.Dispose();
            DialogContent = null;
        }
    }
}
