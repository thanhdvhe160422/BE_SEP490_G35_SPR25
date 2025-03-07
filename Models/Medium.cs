using System;
using System.Collections.Generic;

namespace Planify_BackEnd.Models;

public partial class Medium
{
    public int Id { get; set; }

    public string MediaUrl { get; set; } = null!;

    public virtual ICollection<EventMedium> EventMedia { get; set; } = new List<EventMedium>();

    public virtual ICollection<InvoiceImagesSubTask> InvoiceImagesSubTasks { get; set; } = new List<InvoiceImagesSubTask>();

    public virtual ICollection<InvoiceImagesTask> InvoiceImagesTasks { get; set; } = new List<InvoiceImagesTask>();

    public virtual ICollection<ReportMedium> ReportMedia { get; set; } = new List<ReportMedium>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
