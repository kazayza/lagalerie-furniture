using System;
using System.Collections.Generic;

namespace LagalerieFurniture.Models;

public partial class Bomitem
{
    public long Id { get; set; }

    public long Bomid { get; set; }

    public long MaterialProductId { get; set; }

    public decimal Quantity { get; set; }

    public string? UnitMeasure { get; set; }

    public decimal UnitCost { get; set; }

    public decimal TotalCost { get; set; }

    public decimal WastagePercent { get; set; }

    public string? Notes { get; set; }

    public int SortOrder { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual BillOfMaterial Bom { get; set; } = null!;

    public virtual Product MaterialProduct { get; set; } = null!;
}
