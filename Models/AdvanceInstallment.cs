using System;
using System.Collections.Generic;

namespace LagalerieFurniture.Models;

public partial class AdvanceInstallment
{
    public long Id { get; set; }

    public long AdvanceId { get; set; }

    public int InstallmentNumber { get; set; }

    public DateOnly DueDate { get; set; }

    public decimal Amount { get; set; }

    public decimal DeductedAmount { get; set; }

    public DateTime? DeductedAt { get; set; }

    public long? PayrollId { get; set; }

    public string Status { get; set; } = null!;

    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual EmployeeAdvance Advance { get; set; } = null!;
}
