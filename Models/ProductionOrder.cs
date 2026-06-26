using System;
using System.Collections.Generic;

namespace LagalerieFurniture.Models;

public partial class ProductionOrder
{
    public long Id { get; set; }

    public string ProductionNumber { get; set; } = null!;

    public long? InvoiceItemId { get; set; }

    public long ProductId { get; set; }

    public long? Bomid { get; set; }

    public int BranchId { get; set; }

    public int Quantity { get; set; }

    public int QuantityCompleted { get; set; }

    public int QuantityDefective { get; set; }

    public string Priority { get; set; } = null!;

    public string Status { get; set; } = null!;

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public decimal? EstimatedHours { get; set; }

    public decimal? ActualHours { get; set; }

    public decimal? EstimatedCost { get; set; }

    public decimal? ActualCost { get; set; }

    public int? WarehouseId { get; set; }

    public string? Notes { get; set; }

    public int CreatedById { get; set; }

    public int? SupervisorId { get; set; }

    public int? StartedById { get; set; }

    public int? CompletedById { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual BillOfMaterial? Bom { get; set; }

    public virtual Branch Branch { get; set; } = null!;

    public virtual User? CompletedBy { get; set; }

    public virtual User CreatedBy { get; set; } = null!;

    public virtual InvoiceItem? InvoiceItem { get; set; }

    public virtual ICollection<MaterialConsumption> MaterialConsumptions { get; set; } = new List<MaterialConsumption>();

    public virtual Product Product { get; set; } = null!;

    public virtual ICollection<ProductionStage> ProductionStages { get; set; } = new List<ProductionStage>();

    public virtual User? StartedBy { get; set; }

    public virtual User? Supervisor { get; set; }

    public virtual Warehouse? Warehouse { get; set; }
}
