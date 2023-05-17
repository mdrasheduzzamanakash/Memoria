namespace MemoriaMVC.ViewModel.Attachment
{
    public class AttachmentViewModel
    {
        public string NoteId { get; set; }
        public string FileType { get; set; }

        public int? ContentSize { get; set; }

        public byte[] file { get; set; }

        public string OwnerId { get; set; }
    }
}
