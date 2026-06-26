using System;
using System.Collections.Generic;

namespace LagalerieFurniture.Models;

public partial class WarehouseLocation
{
    public int Id { get; set; }

    public int WarehouseId { get; set; }

    public string Aisle { get; set; } = null!;

    public string Rack { get; set; } = null!;

    public string? Shelf { get; set; }

    public string? Bin { get; set; }

    public string FullLocation { get; set; } = null!;

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();

    public virtual Warehouse Warehouse { get; set; } = null!;
}
