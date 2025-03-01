using System;
using System.Collections.Generic;

namespace Planify_BackEnd.Models;

public partial class ReportMedium
{
    public int Id { get; set; }

    public int? ReportId { get; set; }

    public string? MediaUrl { get; set; }

    public virtual Report? Report { get; set; }
}
