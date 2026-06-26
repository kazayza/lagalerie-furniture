using System;
using System.Collections.Generic;

namespace LagalerieFurniture.Models;

public partial class JournalEntry
{
    public long Id { get; set; }

    public string EntryNumber { get; set; } = null!;

    public DateTime EntryDate { get; set; }

    public string Description { get; set; } = null!;

    public string? ReferenceType { get; set; }

    public long? ReferenceId { get; set; }

    public decimal TotalDebit { get; set; }

    public decimal TotalCredit { get; set; }

    public string Status { get; set; } = null!;

    public bool IsReversing { get; set; }

    public long? ReversedEntryId { get; set; }

    public int? FiscalPeriodId { get; set; }

    public string? Notes { get; set; }

    public int CreatedById { get; set; }

    public int? ApprovedById { get; set; }

    public DateTime? ApprovedAt { get; set; }

    public DateTime? PostedAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual User? ApprovedBy { get; set; }

    public virtual ICollection<AssetDepreciation> AssetDepreciations { get; set; } = new List<AssetDepreciation>();

    public virtual ICollection<AssetTransaction> AssetTransactions { get; set; } = new List<AssetTransaction>();

    public virtual User CreatedBy { get; set; } = null!;

    public virtual FiscalPeriod? FiscalPeriod { get; set; }

    public virtual ICollection<JournalEntry> InverseReversedEntry { get; set; } = new List<JournalEntry>();

    public virtual ICollection<JournalEntryLine> JournalEntryLines { get; set; } = new List<JournalEntryLine>();

    public virtual JournalEntry? ReversedEntry { get; set; }
}
