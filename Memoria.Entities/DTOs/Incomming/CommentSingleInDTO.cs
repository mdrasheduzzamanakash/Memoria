using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memoria.Entities.DTOs.Incomming
{
    public class CommentSingleInDTO
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
