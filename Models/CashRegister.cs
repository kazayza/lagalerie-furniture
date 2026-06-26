using System;
using System.Collections.Generic;

namespace LagalerieFurniture.Models;

public partial class CashRegister
{
    public int Id { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public int BranchId { get; set; }

    public int AccountId { get; set; }

    public string Currency { get; set; } = null!;

    public decimal OpeningBalance { get; set; }

    public decimal CurrentBalance { get; set; }

    public decimal MinimumBalance { get; set; }

    public decimal? MaximumBalance { get; set; }

    public bool IsMain { get; set; }

    public bool IsActive { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedAt { get; set; }

    public int? DeletedById { get; set; }

    public int? ManagerId { get; set; }

    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<BankDeposit> BankDeposits { get; set; } = new List<BankDeposit>();

    public virtual Branch Branch { get; set; } = null!;

    public virtual ICollection<CashRegisterDailySettlement> CashRegisterDailySettlements { get; set; } = new List<CashRegisterDailySettlement>();

    public virtual ICollection<CashRegisterTransaction> CashRegisterTransactions { get; set; } = new List<CashRegisterTransaction>();

    public virtual ICollection<CashTransfer> CashTransferFromCashRegisters { get; set; } = new List<CashTransfer>();

    public virtual ICollection<CashTransfer> CashTransferToCashRegisters { get; set; } = new List<CashTransfer>();

    public virtual User? Manager { get; set; }

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
