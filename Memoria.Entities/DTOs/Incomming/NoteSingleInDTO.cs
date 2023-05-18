namespace Memoria.Entities.DTOs.Incomming
{
    public class NoteSingleInDTO
    {
        public string? Id { get; set; }
        public int? Status { get; set; }
        public System.DateTime? UpdatedDateAndTime { get; set; }
        public string? UpdatedBy { get; set; }
        public string? AddedBy { get; set; }
        public string? FileFormat { get; set; }
        public string? AuthorId { get; set; }
        public string? Type { get; set; }
        public string? Title { get; set; }

        public string? Description { get; set; }

        public string? Todos { get; set; }

        public string? Labels { get; set; }

        public System.DateTime? TrashingDate { get; set; }


        public string? BgColor { get; set; }

        public bool IsHidden { get; set; }

        public bool IsTrashed { get; set; }

        public bool IsArchived { get; set; }
        public bool IsPinned { get; set; }

        public bool IsMarked { get; set; }

        public bool IsDraft { get; set; }

        public bool IsArchieved { get; set; }

        public bool IsRemainderAdded { get; set; }

        public System.DateTime? RemainderDateTime { get; set; }
    }
}
