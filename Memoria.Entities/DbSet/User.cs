using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memoria.Entities.DbSet
{
    public class User : BaseEntity
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }
        
        public byte[]? Image { get; set; }

        public Guid IdentityId { get; set; }

        public string? ActiveEditingNote { get; set; }

        public string? uniqueEmailVerificationToken { get; set;}

        public bool? isEmailVerified { get; set; }
    }
}
