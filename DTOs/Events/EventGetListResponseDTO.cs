using Planify_BackEnd.DTOs.Campus;
using Planify_BackEnd.DTOs.Categories;
using Planify_BackEnd.DTOs.Medias;

namespace Planify_BackEnd.DTOs.Events
{
    public class EventGetListResponseDTO
    {
        public int Id { get; set; }

        public string EventTitle { get; set; } = null!;

        public string? EventDescription { get; set; }

        public Guid? CreateBy { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public DateTime? TimeOfEvent { get; set; }

        public DateTime? EndOfEvent { get; set; }

        public DateTime? CreatedAt { get; set; }

        public decimal? AmountBudget { get; set; }

        public int? IsPublic { get; set; }

        public DateTime? TimePublic { get; set; }

        public int? Status { get; set; }

        public Guid? ManagerId { get; set; }

        public int? CampusId { get; set; }

        public int? CategoryEventId { get; set; }

        public string? Placed { get; set; }

        public List<string> MediaUrls { get; set; }
    }
}
