using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memoria.Entities.DbSet
{
    public class Comment : BaseEntity
    {
        public string Content { get; set; }
        public DateTime EditedDateAndTime { get; set; }

        public string NoteId { get; set; }

        [ForeignKey("User")]
        [Required]
        public string CommenterId { get; set; }

        public virtual User Commenter { get; set; }
    }
}
