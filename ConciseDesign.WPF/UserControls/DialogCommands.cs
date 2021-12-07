using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ConciseDesign.WPF.UserControls
{
    /// <summary>
    /// must use flowing command if use <see cref="DialogHostControl"/>
    /// </summary>
    public static class DialogCommands
    {
        public static readonly RoutedCommand OpenDialogCommand = new RoutedCommand("Open", typeof(DialogCommands));

        public static readonly RoutedCommand CloseDialogCommand = new RoutedCommand("Close", typeof(DialogCommands));

        public static readonly RoutedCommand SubmitDialogCommand = new RoutedCommand("Submit", typeof(DialogCommands));

        public static readonly RoutedCommand CancelDialogCommand = new RoutedCommand("Cancel", typeof(DialogCommands));

        public static void CancelDialog(this Control userControl, object parameter = null)
        {
            CancelDialogCommand.Execute(parameter, userControl);
        }

        public static void CloseDialog(this Control control, object parameter = null)
        {
            CloseDialogCommand.Execute(parameter, control);
        }

        public static void SubmitDialog(this Control control, object parameter = null)
        {
            SubmitDialogCommand.Execute(parameter, control);
        }

        public static void OpenDialog(this Control control, object parameter = null)
        {
            OpenDialogCommand.Execute(parameter, control);
        }

        public static TaskAwaiter<bool> GetAwaiter(this DialogEntry dialogEntry)
        {
            return dialogEntry.TaskCompletionSource.Task.GetAwaiter();
        }
    }

    public enum DialogAction
    {
        /// <summary>
        /// 打开
        /// </summary>
        Open,

        /// <summary>
        /// 关闭
        /// </summary>
        Close,

        /// <summary>
        /// 表示取消
        /// </summary>
        Cancel,

        /// <summary>
        /// 表示提交
        /// </summary>
        Submit,
    }
}