﻿using System.ComponentModel.DataAnnotations;
using Planify_BackEnd.Models;

namespace Planify_BackEnd.DTOs.Tasks
{
    public class TaskCreateRequestDTO
    {
        [Required]
        public string TaskName { get; set; }

        public string TaskDescription { get; set; }

        public DateTime CreateDate { get; set; }
        [Required]
        public DateTime StartTime { get; set; }
        [Required]
        public DateTime Deadline { get; set; }

        public decimal AmountBudget { get; set; }

        public Guid OrganizerId { get; set; } // Nguoi tao tasks

        public TaskCreateRequestDTO() { }
    }
}
