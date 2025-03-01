using System;
using System.Collections.Generic;

namespace Planify_BackEnd.Models;

public partial class SubTask
{
    public int Id { get; set; }

    public int? TaskId { get; set; }

    public Guid? CreateBy { get; set; }

    public string? SubTaskName { get; set; }

    public string? SubTaskDescription { get; set; }

    public DateTime? StartTime { get; set; }

    public DateTime? Deadline { get; set; }

    public decimal? AmountBudget { get; set; }

    public int? Status { get; set; }

    public virtual ICollection<AssignTask> AssignTasks { get; set; } = new List<AssignTask>();

    public virtual User? CreateByNavigation { get; set; }

    public virtual ICollection<InvoiceImagesSubTask> InvoiceImagesSubTasks { get; set; } = new List<InvoiceImagesSubTask>();

    public virtual Task? Task { get; set; }
}
