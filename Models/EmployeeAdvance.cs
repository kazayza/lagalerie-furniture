using System;
using System.Collections.Generic;

namespace LagalerieFurniture.Models;

public partial class EmployeeAdvance
{
    public long Id { get; set; }

    public string AdvanceNumber { get; set; } = null!;

    public int EmployeeId { get; set; }

    public DateTime RequestDate { get; set; }

    public decimal Amount { get; set; }

    public string Reason { get; set; } = null!;

    public int InstallmentCount { get; set; }

    public decimal InstallmentAmount { get; set; }

    public DateOnly StartDeductionMonth { get; set; }

    public string Status { get; set; } = null!;

    public int? ApprovedById { get; set; }

    public DateTime? ApprovedAt { get; set; }

    public int? PaidById { get; set; }

    public DateTime? PaidAt { get; set; }

    public string? PaymentMethod { get; set; }

    public string? Notes { get; set; }

    public int CreatedById { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<AdvanceInstallment> AdvanceInstallments { get; set; } = new List<AdvanceInstallment>();

    public virtual User? ApprovedBy { get; set; }

    public virtual User CreatedBy { get; set; } = null!;

    public virtual Employee Employee { get; set; } = null!;

    public virtual User? PaidBy { get; set; }
}
