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
}