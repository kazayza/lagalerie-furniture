using System;
using System.Collections.Generic;

namespace LagalerieFurniture.Models;

public partial class StockCountItem
{
    public long Id { get; set; }

    public long StockCountId { get; set; }

    public long ProductVariantId { get; set; }

    public decimal SystemQuantity { get; set; }

    public decimal PhysicalQuantity { get; set; }

    public decimal Difference { get; set; }

    public bool IsBundle { get; set; }

    public long? BundleId { get; set; }

    public string? Reason { get; set; }

    public int CountedById { get; set; }

    public DateTime CountedAt { get; set; }

    public virtual Product? Bundle { get; set; }

    public virtual User CountedBy { get; set; } = null!;

    public virtual ProductVariant ProductVariant { get; set; } = null!;

    public virtual StockCount StockCount { get; set; } = null!;
}
