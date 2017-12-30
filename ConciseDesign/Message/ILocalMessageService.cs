using System.Threading;

namespace ConciseDesign.WPF.Message
{
    /// <summary>
    /// 本地通知服务
    /// </summary>
    public interface ILocalMessageService
    {
        void Raise(string msg, AlertType alertType);

        void Raise(IAlertMessage message);

        void Warning(string msg);

        void Message(string msg);
    }

    public class LocalMessageService : ILocalMessageService
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