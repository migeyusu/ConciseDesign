using System.Windows.Controls;
using ConciseDesign.WPF.Command;
using ConciseDesign.WPF.Message;
using ConciseDesign.WPF.UserControls;

namespace Demo
{
    public partial class DialogTest : UserControl
    {
        public DialogTest()
        {
            InitializeComponent();
        }
    }

    public class DialogViewModel
    {
        private readonly UserControl _userControl;

        public DialogViewModel(UserControl userControl)
        {
            _userControl = userControl;
        }

        public BaseCommand BaseCommand => new BaseCommand((o =>
        {
            var desktopMessageService = new DesktopMessageService();
            desktopMessageService.Raise("asdf",AlertType.Warning);
            // _userControl.CancelDialog();
        }));
    }
}