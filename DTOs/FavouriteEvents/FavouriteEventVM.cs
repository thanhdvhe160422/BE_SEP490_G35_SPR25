﻿using Planify_BackEnd.Models;

namespace Planify_BackEnd.DTOs.FavouriteEvents
{
    public class FavouriteEventVM
    {
        public int Id { get; set; }

        public string EventTitle { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public string Placed { get; set; }
        public int? Status { get; set; }
        public ICollection<EventMediaDto> EventMedia { get; set; }
        public string? StatusMessage
        {
            get
            {
                if (StartTime.HasValue && StartTime.Value <= DateTime.Now && EndTime.HasValue && EndTime.Value >= DateTime.Now)
                {
                    return "Running";
                }
                else if (StartTime.HasValue && StartTime.Value > DateTime.Now)
                {
                    return "Not Start Yet";
                }
                else if (EndTime.HasValue && EndTime.Value < DateTime.Now)
                {
                    return "Closed";
                }
                return string.Empty;
            }
        }
    }
}
