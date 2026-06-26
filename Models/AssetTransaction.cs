using System;
using System.Collections.Generic;

namespace LagalerieFurniture.Models;

public partial class AssetTransaction
{
    public long Id { get; set; }

    public long AssetId { get; set; }

    public string TransactionType { get; set; } = null!;

    public DateTime TransactionDate { get; set; }

    public decimal Amount { get; set; }

    public string Description { get; set; } = null!;

    public long? ReferenceId { get; set; }

    public long? JournalEntryId { get; set; }

    public int CreatedById { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual FixedAsset Asset { get; set; } = null!;

    public virtual User CreatedBy { get; set; } = null!;

    public virtual JournalEntry? JournalEntry { get; set; }
}
