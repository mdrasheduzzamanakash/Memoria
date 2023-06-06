using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memoria.Entities.DTOs.Incomming
{
    public class RefreshTokenSingleInDTO
    {
        public string Id { get; set; }
        public Guid? IdentityId { get; set; }
        public int? Status { get; set; }
        public System.DateTime? UpdatedDateAndTime { get; set; }
        public string? UpdatedBy { get; set; }
        public string? AddedBy { get; set; }
        public string? FileFormat { get; set; }
        public string UserId { get; set; }
        public string Token { get; set; }

        public string JwtId { get; set; }

        public bool IsUsed { get; set; }

        public bool IsRevoked { get; set; }

        public DateTime ExpiryDate { get; set; }
    }
}
