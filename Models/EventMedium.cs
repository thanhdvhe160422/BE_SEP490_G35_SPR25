using System;
using System.Collections.Generic;

namespace Planify_BackEnd.Models;

public partial class EventMedium
{
    public int Id { get; set; }

    public int? EventId { get; set; }

    public int? MediaId { get; set; }

    public int Status { get; set; }

    public virtual Event? Event { get; set; }

    public virtual MediaItem? Media { get; set; }
}
