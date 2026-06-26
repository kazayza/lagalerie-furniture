using System;
using System.Collections.Generic;

namespace LagalerieFurniture.Models;

public partial class Budget
{
    public long Id { get; set; }

    public int BudgetYear { get; set; }

    public int AccountId { get; set; }

    public int? BranchId { get; set; }

    public decimal PlannedAmount { get; set; }

    public decimal ActualAmount { get; set; }

    public decimal Variance { get; set; }

    public string Status { get; set; } = null!;

    public int CreatedById { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ChartOfAccount Account { get; set; } = null!;

    public virtual Branch? Branch { get; set; }

    public virtual User CreatedBy { get; set; } = null!;
}
