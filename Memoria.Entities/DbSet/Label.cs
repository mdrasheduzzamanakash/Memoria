using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memoria.Entities.DbSet
{
    public class Label : BaseEntity
    {
        public string Content {  get; set; }

        [ForeignKey("User")]
        public string LabelerId { get; set; }
        public virtual User User { get; set; }

    }
}
