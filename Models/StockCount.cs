using System;
using System.Collections.Generic;

namespace LagalerieFurniture.Models;

public partial class StockCount
{
    public long Id { get; set; }

    public string CountNumber { get; set; } = null!;

    public int WarehouseId { get; set; }

    public DateTime CountDate { get; set; }

    public string CountType { get; set; } = null!;

    public string Status { get; set; } = null!;

    public string? Notes { get; set; }

    public int StartedById { get; set; }

    public int? ApprovedById { get; set; }

    public DateTime? ApprovedAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual User? ApprovedBy { get; set; }

    public virtual User StartedBy { get; set; } = null!;

    public virtual ICollection<StockCountAdjustment> StockCountAdjustments { get; set; } = new List<StockCountAdjustment>();

    public virtual ICollection<StockCountItem> StockCountItems { get; set; } = new List<StockCountItem>();

    public virtual Warehouse Warehouse { get; set; } = null!;
}
