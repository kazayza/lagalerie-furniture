using System;
using System.Collections.Generic;

namespace LagalerieFurniture.Models;

public partial class AssetDepreciation
{
    public long Id { get; set; }

    public long AssetId { get; set; }

    public int PeriodYear { get; set; }

    public int PeriodMonth { get; set; }

    public decimal DepreciationAmount { get; set; }

    public decimal AccumulatedValue { get; set; }

    public long? JournalEntryId { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual FixedAsset Asset { get; set; } = null!;

    public virtual JournalEntry? JournalEntry { get; set; }
}
