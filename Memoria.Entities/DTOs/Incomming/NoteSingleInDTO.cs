using Memoria.Entities.DbSet;
using Memoria.Entities.DTOs.Outgoing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memoria.Entities.DTOs.Incomming
{
    public class NoteSingleInDTO : BaseDTO
    {
        public string? Type { get; set; }
        public string? Title { get; set; }

        public string? Description { get; set; }

        public string? Todos { get; set; }

        public System.DateTime? TrashingDate { get; set; }


        public string? BgColor { get; set; }

        public bool IsHidden { get; set; }

        public bool IsTrashed { get; set; }

        public bool IsArchived { get; set; }
        public bool IsPinned { get; set; }

        public bool IsMarked { get; set; }

        public bool IsDraft { get; set; }

        public bool IsArchieved { get; set; }

        public bool IsRemainderAdded { get; set; }


        public System.DateTime? RemainderDateTime { get; set; }


        public User User { get; set; }

        public ICollection<NoteLabel>? Labels { get; set; }
    }
}
