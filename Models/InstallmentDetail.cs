using System;
using System.Collections.Generic;

namespace LagalerieFurniture.Models;

public partial class InstallmentDetail
{
    public long Id { get; set; }

    public long InstallmentId { get; set; }

    public int InstallmentNumber { get; set; }

    public DateTime DueDate { get; set; }

    public decimal Amount { get; set; }

    public decimal PaidAmount { get; set; }

    public DateTime? PaidDate { get; set; }

    public string Status { get; set; } = null!;

    public decimal LateFee { get; set; }

    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Installment Installment { get; set; } = null!;
}
