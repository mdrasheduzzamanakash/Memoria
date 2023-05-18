using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memoria.Entities.DbSet
{
    public class Authorization : BaseEntity
    {
        public string NoteId { get; set; }

        public bool IsValid { get; set; }

        public string ModeOfAuthorization { get; set; }

        [ForeignKey("User")]
        public string AuthorizerId { get; set; }
        public virtual User AuthorizedUser { get; set; }
        public virtual User Authorizer { get; set; }
    }
}
