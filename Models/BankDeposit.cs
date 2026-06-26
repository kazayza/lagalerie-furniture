using System;
using System.Collections.Generic;

namespace LagalerieFurniture.Models;

public partial class BankDeposit
{
    public long Id { get; set; }

    public string DepositNumber { get; set; } = null!;

    public int CashRegisterId { get; set; }

    public int BankAccountId { get; set; }

    public decimal Amount { get; set; }

    public DateTime DepositDate { get; set; }

    public string? ReferenceNumber { get; set; }

    public string Status { get; set; } = null!;

    public string? Notes { get; set; }

    public int CreatedById { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual CashRegister CashRegister { get; set; } = null!;

    public virtual User CreatedBy { get; set; } = null!;
}
