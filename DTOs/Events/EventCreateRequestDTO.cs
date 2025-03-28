﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Planify_BackEnd.DTOs.Events
{
    public class EventCreateRequestDTO
    {
        [Required]
        public string EventTitle { get; set; }

        public string EventDescription { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        [Required]
        public int CategoryEventId { get; set; }

        public string Placed { get; set; }

        public string MeasuringSuccess { get; set; }

        public string Goals { get; set; }

        public string MonitoringProcess { get; set; }

        [Range(0, int.MaxValue)]
        public int SizeParticipants { get; set; }

        public string PromotionalPlan { get; set; }

        public string TargetAudience { get; set; }

        public string SloganEvent { get; set; }

        public List<TaskCreateDTO> Tasks { get; set; }

        public List<RiskCreateDTO> Risks { get; set; }

        public List<CostBreakdownCreateDTO> CostBreakdowns { get; set; }
    }

    public class TaskCreateDTO
    {
        [Required]
        public string TaskName { get; set; }

        public string Description { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? Deadline { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Budget { get; set; }

        public List<SubTaskCreateDTO> SubTasks { get; set; }
    }

    public class SubTaskCreateDTO
    {
        [Required]
        public string SubTaskName { get; set; }

        public string Description { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? Deadline { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Budget { get; set; }
    }

    public class RiskCreateDTO
    {
        [Required]
        public string Name { get; set; }

        public string Reason { get; set; }

        public string Solution { get; set; }

        public string Description { get; set; }
    }

    public class CostBreakdownCreateDTO
    {
        [Required]
        public string Name { get; set; }

        [Range(0, int.MaxValue)]
        public int? Quantity { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? PriceByOne { get; set; }
    }
}