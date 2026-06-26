using System;
using System.Collections.Generic;

namespace LagalerieFurniture.Models;

public partial class CashRegisterTransaction
{
    public long Id { get; set; }

    public string TransactionNumber { get; set; } = null!;

    public int CashRegisterId { get; set; }

    public string TransactionType { get; set; } = null!;

    public string? ReferenceType { get; set; }

    public long? ReferenceId { get; set; }

    public decimal Amount { get; set; }

    public string Direction { get; set; } = null!;

    public decimal PreviousBalance { get; set; }

    public decimal NewBalance { get; set; }

    public string Description { get; set; } = null!;

    public string? Notes { get; set; }

    public int CreatedById { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual CashRegister CashRegister { get; set; } = null!;

    public virtual User CreatedBy { get; set; } = null!;
}
