using Microsoft.Build.Framework;

namespace MemoriaMVC.SocketConnections.Models.Incomming
{
    public class NoteChangeSingleInModel
    {

        public string NoteId { get; set; }

        public string WriterId { get; set; }

        public string TitleChanges { get; set; }

        public string DescriptionChanges { get; set; }

        public bool IsTitleChanged { get; set; } = false;

        public bool IsDescriptionChanged { get; set; } = false;
    }
}
