using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memoria.Entities.DTOs.Incomming
{
    public class AttachmentSingleInDTO
    {
        public string Id { get; set; }
        public int? Status { get; set; }
        public System.DateTime? UpdatedDateAndTime { get; set; }
        public string? UpdatedBy { get; set; }
        public string? AddedBy { get; set; }
        public string? FileFormat { get; set; }

        public string NoteId { get; set; }
        public string FileType { get; set; }

        public int? ContentSize { get; set; }

        public byte[] file { get; set; }

        public string OwnerId { get; set; }
    }
}
