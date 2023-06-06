using System.Drawing.Drawing2D;

namespace MemoriaMVC.SocketConnections.Models.Incomming
{
    public class NoteCommentSingleInModel
    {
        public string NoteId { get; set; }
        public string CommenterId { get; set; }
        public string Content { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Image { get; set; }

        public string FileFormat { get; set; }
    }
}