using System.Windows.Input;

namespace ConciseDesign.WPF.UserControls
{
    public static class Commands
    {
        public static readonly RoutedCommand OpenDialogCommand = new RoutedCommand("Open", typeof(Commands));

        public static readonly RoutedCommand CloseDialogCommand = new RoutedCommand("Close", typeof(Commands));
            
        public static readonly RoutedCommand SubmitDialogCommand = new RoutedCommand("Submit", typeof(Commands));

        public static readonly RoutedCommand CancelDialogCommand = new RoutedCommand("Cancel", typeof(Commands));
    }
}