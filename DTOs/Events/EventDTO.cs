﻿using Planify_BackEnd.Models;

namespace Planify_BackEnd.DTOs.Events
{
    public class EventDTO
    {
        public int Id { get; set; }

        public string EventTitle { get; set; }

        public string EventDescription { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public decimal AmountBudget { get; set; }

        public int IsPublic { get; set; }

        public DateTime? TimePublic { get; set; }

        public int Status { get; set; }

        public Guid? ManagerId { get; set; }

        public int CampusId { get; set; }

        public int CategoryEventId { get; set; }

        public string Placed { get; set; }

        public DateTime CreatedAt { get; set; }

        public Guid CreateBy { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public Guid? UpdateBy { get; set; }
    }
}
