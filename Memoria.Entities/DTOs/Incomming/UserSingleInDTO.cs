namespace Memoria.Entities.DTOs.Incomming
{
    public class UserSingleInDTO
    {
        public string Id { get; set; }
        public Guid? IdentityId { get; set; }
        public int? Status { get; set; }
        public System.DateTime? UpdatedDateAndTime { get; set; }
        public string? UpdatedBy { get; set; }
        public string? AddedBy { get; set; }
        public string? FileFormat { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public byte[] Image { get; set; }

        public string? ActiveEditingNote { get; set; }
    }
}
