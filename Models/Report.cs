using System;
using System.Collections.Generic;

namespace Planify_BackEnd.Models;

public partial class Report
{
    public int Id { get; set; }

    public Guid SendFrom { get; set; }

    public int TaskId { get; set; }

    public string Reason { get; set; } = null!;

    public Guid ReportUserId { get; set; }

    public DateTime SendTime { get; set; }

    public int Status { get; set; }

    public virtual ICollection<ReportMedium> ReportMedia { get; set; } = new List<ReportMedium>();

    public virtual User ReportUser { get; set; } = null!;

    public virtual User SendFromNavigation { get; set; } = null!;

    public virtual Task Task { get; set; } = null!;
}
