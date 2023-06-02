using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memoria.Entities.DTOs.Outgoing
{
    public class AuthorizationSingleOutDTO
    {
        public string Id { get; set; }
        public int Status { get; set; } = 1;
        public DateTime? AddedDateAndTime { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedDateAndTime { get; set; } = DateTime.UtcNow;
        public string? UpdatedBy { get; set; }
        public string? AddedBy { get; set; }
        public string? FileFormat { get; set; }
        public string NoteId { get; set; }

        public bool IsValid { get; set; }

        public string ModeOfAuthorization { get; set; }

        public string AuthorizerId { get; set; }
        public string AuthorizedUserId { get; set; }
    }
}
