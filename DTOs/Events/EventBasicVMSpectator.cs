using Planify_BackEnd.DTOs.Campus;
using Planify_BackEnd.DTOs.Categories;
using Planify_BackEnd.DTOs.Medias;

namespace Planify_BackEnd.DTOs.Events
{
    public class EventBasicVMSpectator
    {
        public int Id { get; set; }

        public string EventTitle { get; set; } = null!;

        public string? EventDescription { get; set; }
        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public int? IsPublic { get; set; }

        public int? Status { get; set; }

        public int? CampusId { get; set; }

        public int? CategoryEventId { get; set; }

        public string? Placed { get; set; } = null!;

        public CampusDTO? CampusDTO { get; set; }

        public CategoryViewModel? CategoryViewModel { get; set; }

        public ICollection<EventMediumViewMediaModel> EventMedias { get; set; } = new List<EventMediumViewMediaModel>();

    }
}
