using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memoria.Entities.DTOs.Outgoing
{
    public class UserCollaboratorSearchResultDto
    {
        public string Id { get; set; }
        public int Status { get; set; } = 1;
        public string? FileFormat { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string fileBase64 { get; set; }
    }
}
