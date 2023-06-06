namespace MemoriaMVC.SocketConnections.Models.Outgoing
{
    public class NoteCommentSingleOutModel
    {
        public string NoteId { get; set; }
        public string CommenterId { get; set; }
        public string Content { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Image { get; set; }
        public string FileFormat { get; set; }
        public DateTime AddedDateAndTime { get; set; }
    }
}
