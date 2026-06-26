using System;
using System.Collections.Generic;

namespace LagalerieFurniture.Models;

public partial class JournalEntryLine
{
    public long Id { get; set; }

    public long JournalEntryId { get; set; }

    public int AccountId { get; set; }

    public decimal Debit { get; set; }

    public decimal Credit { get; set; }

    public string? Description { get; set; }

    public int? CostCenterId { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ChartOfAccount Account { get; set; } = null!;

    public virtual CostCenter? CostCenter { get; set; }

    public virtual JournalEntry JournalEntry { get; set; } = null!;
}
