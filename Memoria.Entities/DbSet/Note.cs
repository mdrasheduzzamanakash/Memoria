using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memoria.Entities.DbSet
{
    public class Note : BaseEntity
    {
        [Required]
        public string Type { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public System.DateTime? TrashingDate { get; set; }

        [Required]
        public string BgColor { get; set; }

        [Required]
        public bool IsHidden { get; set; }

        [Required]
        public bool IsTrashed { get; set; }

        [Required]
        public bool IsArchived { get; set; }

        [Required]
        public bool IsRemainderAdded { get; set; }

        [Required]
        public System.DateTime? RemainderDateTime { get; set; }

        [Required]
        public User User { get; set; }

        public ICollection<NoteLabel>? Labels { get; set; }
    }
}
