namespace ConciseDesign.WPF.Message
{
    /// <summary>
    /// 轻量级消息，用于提醒
    /// </summary>
    public interface IAlertMessage
    {
        string Message { get; set; }

        AlertType AlertType { get; set; }
    }
}