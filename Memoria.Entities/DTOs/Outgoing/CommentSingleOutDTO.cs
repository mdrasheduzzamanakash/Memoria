using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memoria.Entities.DTOs.Outgoing
{
    public class CommentSingleOutDTO
    {
        public string Id { get; set; }
        public int Status { get; set; } = 1;
        public DateTime? UpdatedDateAndTime { get; set; }
        public string? UpdatedBy { get; set; }
        public string? AddedBy { get; set; }
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
