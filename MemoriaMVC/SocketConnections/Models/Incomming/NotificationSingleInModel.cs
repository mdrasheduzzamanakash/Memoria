namespace MemoriaMVC.SocketConnections.Models.Incomming
{
    public class NotificationSingleInModel
    {
        public string SenderId { get; set; }

        public string ReceiverId { get; set; }

        public string Content { get; set; }

        public string link { get; set; }

        public string NoticeState { get; set; }

        public bool IsSent { get; set; }
    }
}
