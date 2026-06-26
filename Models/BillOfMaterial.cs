using System;
using System.Collections.Generic;

namespace LagalerieFurniture.Models;

public partial class BillOfMaterial
{
    public long Id { get; set; }

    public long ProductId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public decimal EstimatedCost { get; set; }

    public decimal EstimatedHours { get; set; }

    public bool IsActive { get; set; }

    public int? CreatedById { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<Bomitem> Bomitems { get; set; } = new List<Bomitem>();

    public virtual User? CreatedBy { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual ICollection<ProductionOrder> ProductionOrders { get; set; } = new List<ProductionOrder>();
}
