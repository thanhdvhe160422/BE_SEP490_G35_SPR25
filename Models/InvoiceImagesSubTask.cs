using System;
using System.Collections.Generic;

namespace Planify_BackEnd.Models;

public partial class InvoiceImagesSubTask
{
    public int Id { get; set; }

    public int SubTaskId { get; set; }

    public int MediaId { get; set; }

    public decimal ActualBudgetAmount { get; set; }

    public int Status { get; set; }

    public virtual Medium Media { get; set; } = null!;

    public virtual SubTask SubTask { get; set; } = null!;
}
