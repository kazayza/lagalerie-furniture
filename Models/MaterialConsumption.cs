using System;
using System.Collections.Generic;

namespace LagalerieFurniture.Models;

public partial class MaterialConsumption
{
    public long Id { get; set; }

    public long ProductionOrderId { get; set; }

    public long? ProductionStageId { get; set; }

    public long ProductVariantId { get; set; }

    public int WarehouseId { get; set; }

    public decimal QuantityPlanned { get; set; }

    public decimal QuantityUsed { get; set; }

    public decimal QuantityWasted { get; set; }

    public decimal UnitCost { get; set; }

    public decimal TotalCost { get; set; }

    public string? Notes { get; set; }

    public int CreatedById { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual User CreatedBy { get; set; } = null!;

    public virtual ProductVariant ProductVariant { get; set; } = null!;

    public virtual ProductionOrder ProductionOrder { get; set; } = null!;

    public virtual ProductionStage? ProductionStage { get; set; }

    public virtual Warehouse Warehouse { get; set; } = null!;
}
