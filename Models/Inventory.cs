using System;
using System.Collections.Generic;

namespace LagalerieFurniture.Models;

public partial class Inventory
{
    public long Id { get; set; }

    public long ProductVariantId { get; set; }

    public int WarehouseId { get; set; }

    public int? WarehouseLocationId { get; set; }

    public decimal QuantityOnHand { get; set; }

    public decimal QuantityReserved { get; set; }

    public decimal QuantityAvailable { get; set; }

    public int ReorderLevel { get; set; }

    public int ReorderQuantity { get; set; }

    public DateTime? LastCountDate { get; set; }

    public decimal UnitCost { get; set; }

    public virtual ProductVariant ProductVariant { get; set; } = null!;

    public virtual Warehouse Warehouse { get; set; } = null!;

    public virtual WarehouseLocation? WarehouseLocation { get; set; }
}
