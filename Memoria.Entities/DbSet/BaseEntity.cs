using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memoria.Entities.DbSet
{
    public abstract class BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public int Status { get; set; } = 1;
        public DateTime AddedDateAndTime { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedDateAndTime { get; set; } = DateTime.UtcNow;
        public string? UpdatedBy { get; set; }
        public string? AddedBy { get; set; }
        public string? FileFormat { get; set; }
    }
}
