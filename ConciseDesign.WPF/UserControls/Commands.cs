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
    }
}