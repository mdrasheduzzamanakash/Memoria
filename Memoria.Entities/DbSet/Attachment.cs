using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memoria.Entities.DbSet
{
    public class Attachment : BaseEntity
    {
        public string NoteId { get; set; }
        public string ContentType { get; set; }

        public int ContentSize { get; set; }

        public byte[] Content { get; set; }

        [ForeignKey("User")]
        [Required]
        public string UserId { get; set; }
        public virtual User User { get; set; }
    }
}
