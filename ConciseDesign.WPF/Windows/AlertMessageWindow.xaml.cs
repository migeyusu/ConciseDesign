using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;
using ConciseDesign.WPF.Message;

namespace ConciseDesign.WPF.Windows
{
    /// <summary>
    /// MessageWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AlertMessageWindow : Window
    {
        public AlertMessageWindow(AlertMessage message)
        {
            this.DataContext = message;
            InitializeComponent();
            this.Left = SystemParameters.PrimaryScreenWidth - this.Width;
            this.Top = SystemParameters.PrimaryScreenHeight - this.Height;
        }

        private void Storyboard_Completed(object sender, System.EventArgs e)
        {
            this.Close();
        }
        
        //public AlertType AlertType
        //{
        //    get { return (AlertType)GetValue(AlerttypeDependencyProperty); }
        //    set { SetValue(AlerttypeDependencyProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty AlerttypeDependencyProperty =
        //    DependencyProperty.Register("AlertType", typeof(AlertType), typeof(MessageWindow),
        //        new PropertyMetadata(Service.AlertType.MESSAGE,((o, args) =>
        //        {
        //            var 
        //        } )));
        
        //public string Message
        //{
        //    get { return (string)GetValue(MessageProperty); }
        //    set { SetValue(MessageProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for Message.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty MessageProperty =
        //    DependencyProperty.Register("Message", typeof(string), typeof(MessageWindow), new PropertyMetadata("无信息"));

        private void MessageWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            //           Task.Delay(1500).GetAwaiter().OnCompleted(Close);
            var storyboard = (Storyboard)FindResource("Storyboard1");
            storyboard.Completed += Storyboard_Completed;
            storyboard.Begin();
        }
    }
}
