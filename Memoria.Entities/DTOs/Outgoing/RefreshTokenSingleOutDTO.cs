using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memoria.Entities.DTOs.Outgoing
{
    public class RefreshTokenSingleOutDTO
    {
        public string Id { get; set; }
        public int Status { get; set; } = 1;
        public DateTime? AddedDateAndTime { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedDateAndTime { get; set; } = DateTime.UtcNow;
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
