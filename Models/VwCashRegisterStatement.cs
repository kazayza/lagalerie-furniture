using System;
using System.Collections.Generic;

namespace LagalerieFurniture.Models;

public partial class VwCashRegisterStatement
{
    public string CashRegisterName { get; set; } = null!;

    public string Code { get; set; } = null!;

    public string TransactionNumber { get; set; } = null!;

    public string TransactionType { get; set; } = null!;

    public string? ReferenceType { get; set; }

    public decimal Amount { get; set; }

    public string Direction { get; set; } = null!;

    public decimal PreviousBalance { get; set; }

    public decimal NewBalance { get; set; }

    public string Description { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public string CreatedBy { get; set; } = null!;
}
