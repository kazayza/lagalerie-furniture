using System;
using System.Collections.Generic;

namespace LagalerieFurniture.Models;

public partial class CashTransfer
{
    public long Id { get; set; }

    public string TransferNumber { get; set; } = null!;

    public int FromCashRegisterId { get; set; }

    public int ToCashRegisterId { get; set; }

    public decimal Amount { get; set; }

    public DateTime TransferDate { get; set; }

    public string Status { get; set; } = null!;

    public string? Notes { get; set; }

    public int CreatedById { get; set; }

    public int? ApprovedById { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? CompletedAt { get; set; }

    public virtual User? ApprovedBy { get; set; }

    public virtual User CreatedBy { get; set; } = null!;

    public virtual CashRegister FromCashRegister { get; set; } = null!;

    public virtual CashRegister ToCashRegister { get; set; } = null!;
}
