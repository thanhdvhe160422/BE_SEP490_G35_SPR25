using Planify_BackEnd.Models;

namespace Planify_BackEnd.DTOs.FavouriteEvents
{
    public class FavouriteEventVM
    {
        public int Id { get; set; }

        public string EventTitle { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public string Placed { get; set; }
        public ICollection<EventMediaDto> EventMedia { get; set; }
    }
}
