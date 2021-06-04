using System.Threading;
using System.Threading.Tasks;

namespace ConciseDesign.WPF.Message
{
    public class DesktopMessageService : ILocalMessageService
    {
        public void Raise(string msg, AlertType alertType)
        {
            Raise(new AlertMessage() {
                Message = msg,
                AlertType = alertType
            });
        }

        public void Raise(IAlertMessage message)
        {
            /*Task.Run(() =>
            {
                var win = new Windows.AlertMessageWindow((AlertMessage) message);
                win.ShowDialog();
            });*/
            var thread = new Thread(() =>
                {
                    var win = new Windows.AlertMessageWindow((AlertMessage)message);
                    win.ShowDialog();
                    System.Windows.Threading.Dispatcher.Run();
                })
                { IsBackground = true };
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        public void Warning(string msg)
        {
            Raise(new AlertMessage() {
                AlertType=AlertType.Warning,
                Message = msg
            });
        }

        public void Message(string msg)
        {
            Raise(new AlertMessage() {
                AlertType = AlertType.Message,
                Message = msg
            });
        }
    }
}