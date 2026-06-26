using System;
using System.Collections.Generic;

namespace LagalerieFurniture.Models;

public partial class InventoryTransaction
{
    public long Id { get; set; }

    public string TransactionNumber { get; set; } = null!;

    public long ProductVariantId { get; set; }

    public int WarehouseId { get; set; }

    public string TransactionType { get; set; } = null!;

    public string? ReferenceType { get; set; }

    public long? ReferenceId { get; set; }

    public decimal Quantity { get; set; }

    public decimal PreviousQuantity { get; set; }

    public decimal NewQuantity { get; set; }

    public decimal? UnitCost { get; set; }

    public decimal? TotalCost { get; set; }

    public string? Reason { get; set; }

    public int CreatedById { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual User CreatedBy { get; set; } = null!;

    public virtual ProductVariant ProductVariant { get; set; } = null!;

    public virtual Warehouse Warehouse { get; set; } = null!;
}
