using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memoria.Entities.DbSet
{
    public class NoteLabel : BaseEntity
    {
        public virtual Note Note { get; set; }

        public virtual User Assigner { get; set; }

        public virtual Label Label { get; set; }
    }
}
