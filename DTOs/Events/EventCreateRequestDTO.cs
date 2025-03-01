using System.ComponentModel.DataAnnotations;

namespace Planify_BackEnd.DTOs.Events
{
    public class EventCreateRequestDTO
    {
        [Required]
        public string EventTitle { get; set; } = null!;

        public string? EventDescription { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        public decimal? AmountBudget { get; set; }

        public bool IsPublic { get; set; } = false;

        public DateTime? TimePublic { get; set; }

        public int? Status { get; set; } = 0; // Mặc định là chưa duyệt

        [Required]
        public int CampusId { get; set; }

        [Required]
        public int CategoryEventId { get; set; }

        public string? Placed { get; set; }

        public Guid OrganizerId { get; set; } // Người tạo sự kiện

        public EventCreateRequestDTO() { }
    }
}
