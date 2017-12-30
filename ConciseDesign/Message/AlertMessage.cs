using System;

namespace ConciseDesign.WPF.Message
{
    [Serializable]
    public class AlertMessage:IAlertMessage
    {
        public AlertType AlertType { get; set; }

        public string Message { get; set; }
    }
}
