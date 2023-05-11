using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memoria.Entities.DbSet
{
    public class Trash : BaseEntity
    {
        public virtual Note Note { get; set; }

        public User Trasher { get; set; }

    }
}
