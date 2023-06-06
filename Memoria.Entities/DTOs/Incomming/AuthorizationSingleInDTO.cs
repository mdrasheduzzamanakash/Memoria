using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memoria.Entities.DTOs.Incomming
{
    public class AuthorizationSingleInDTO
    {
        public string NoteId { get; set; }

        public bool IsValid { get; set; }

        public string ModeOfAuthorization { get; set; }

        public string AuthorizerId { get; set; }
        public string AuthorizedUserId { get; set; }
    }
}
