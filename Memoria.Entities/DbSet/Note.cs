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
        public string? AuthorId { get; set; }
        public string? Type { get; set; }
        public string? Title { get; set; }

        public string? Description { get; set; }
        
        public string? Todos { get; set; }

        public System.DateTime? TrashingDate { get; set; }

        
        public string? BgColor { get; set; }

        public bool IsHidden { get; set; } = false;

        public bool IsTrashed { get; set; } = false;

        public bool IsArchived { get; set; } = false;

        public bool IsPinned { get; set; } = false;

        public bool IsMarked { get; set; } = false;

        public bool IsDraft { get; set; } = true;

        public bool IsArchieved { get; set; } = false;

        public bool IsRemainderAdded { get; set; } = false;

        public System.DateTime? RemainderDateTime { get; set; }
    }
}
