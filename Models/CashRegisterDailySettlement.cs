using System;
using System.Collections.Generic;

namespace LagalerieFurniture.Models;

public partial class CashRegisterDailySettlement
{
    public long Id { get; set; }

    public int CashRegisterId { get; set; }

    public DateOnly SettlementDate { get; set; }

    public decimal OpeningBalance { get; set; }

    public decimal ClosingBalance { get; set; }

    public decimal TotalInflow { get; set; }

    public decimal TotalOutflow { get; set; }

    public decimal ExpectedBalance { get; set; }

    public decimal ActualBalance { get; set; }

    public decimal Difference { get; set; }

    public string Status { get; set; } = null!;

    public int? SettledById { get; set; }

    public DateTime? SettledAt { get; set; }

    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual CashRegister CashRegister { get; set; } = null!;

    public virtual User? SettledBy { get; set; }
}
