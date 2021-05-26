using System.Windows.Controls;
using ConciseDesign.WPF.Command;
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
            _userControl.CancelDialog();
        }));
    }
}