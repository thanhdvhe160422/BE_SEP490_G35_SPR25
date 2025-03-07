using System;
using System.Collections.Generic;

namespace Planify_BackEnd.Models;

public partial class InvoiceImagesTask
{
    public int Id { get; set; }

    public int TaskId { get; set; }

    public int MediaId { get; set; }

    public decimal ActualBudgetAmount { get; set; }

    public int Status { get; set; }

    public virtual Medium Media { get; set; } = null!;

    public virtual Task Task { get; set; } = null!;
}
