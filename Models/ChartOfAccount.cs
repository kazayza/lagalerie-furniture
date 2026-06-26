using System;
using System.Collections.Generic;

namespace LagalerieFurniture.Models;

public partial class ChartOfAccount
{
    public int Id { get; set; }

    public string AccountCode { get; set; } = null!;

    public string AccountName { get; set; } = null!;

    public string? AccountNameEn { get; set; }

    public int? ParentAccountId { get; set; }

    public string AccountType { get; set; } = null!;

    public string? AccountCategory { get; set; }

    public string NormalBalance { get; set; } = null!;

    public bool IsSystem { get; set; }

    public bool IsActive { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedAt { get; set; }

    public int? DeletedById { get; set; }

    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<Budget> Budgets { get; set; } = new List<Budget>();

    public virtual ICollection<ChartOfAccount> InverseParentAccount { get; set; } = new List<ChartOfAccount>();

    public virtual ICollection<JournalEntryLine> JournalEntryLines { get; set; } = new List<JournalEntryLine>();

    public virtual ChartOfAccount? ParentAccount { get; set; }
}
