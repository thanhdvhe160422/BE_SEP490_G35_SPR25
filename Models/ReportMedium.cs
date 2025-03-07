using System;
using System.Collections.Generic;

namespace Planify_BackEnd.Models;

public partial class ReportMedium
{
    public int Id { get; set; }

    public int ReportId { get; set; }

    public int MediaId { get; set; }

    public virtual Medium Media { get; set; } = null!;

    public virtual Report Report { get; set; } = null!;
}
