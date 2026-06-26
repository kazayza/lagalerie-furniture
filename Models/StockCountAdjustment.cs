using System;
using System.Collections.Generic;

namespace LagalerieFurniture.Models;

public partial class StockCountAdjustment
{
    public long Id { get; set; }

    public long StockCountId { get; set; }

    public long ProductVariantId { get; set; }

    public decimal OldQuantity { get; set; }

    public decimal NewQuantity { get; set; }

    public decimal AdjustmentAmount { get; set; }

    public string? Reason { get; set; }

    public int CreatedById { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual User CreatedBy { get; set; } = null!;

    public virtual ProductVariant ProductVariant { get; set; } = null!;

    public virtual StockCount StockCount { get; set; } = null!;
}
