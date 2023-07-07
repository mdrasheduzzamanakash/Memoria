using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memoria.Entities.DTOs.Outgoing
{
    public class UserSingleOutDTO
    {
        public string Id { get; set; }
        public int Status { get; set; } = 1;
        public DateTime AddedDateAndTime { get; set; }
        public DateTime UpdatedDateAndTime { get; set; }
        public string UpdatedBy { get; set; }
        public string AddedBy { get; set; }
        public string? FileFormat { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public byte[] Image { get; set; }

        public string? ActiveEditingNote { get; set; }
        public bool? isEmailVerified { get; set; }

        public string? uniqueEmailVerificationToken { get; set; }

    }
}
