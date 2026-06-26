using System;
using System.Collections.Generic;

namespace LagalerieFurniture.Models;

public partial class InventoryTransfer
{
    public long Id { get; set; }

    public string TransferNumber { get; set; } = null!;

    public int? FromBranchId { get; set; }

    public int? ToBranchId { get; set; }

    public int FromWarehouseId { get; set; }

    public int ToWarehouseId { get; set; }

    public DateTime TransferDate { get; set; }

    public string Status { get; set; } = null!;

    public string? Notes { get; set; }

    public int CreatedById { get; set; }

    public int? ApprovedById { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual User? ApprovedBy { get; set; }

    public virtual User CreatedBy { get; set; } = null!;

    public virtual Branch? FromBranch { get; set; }

    public virtual Warehouse FromWarehouse { get; set; } = null!;

    public virtual ICollection<InventoryTransferItem> InventoryTransferItems { get; set; } = new List<InventoryTransferItem>();

    public virtual Branch? ToBranch { get; set; }

    public virtual Warehouse ToWarehouse { get; set; } = null!;
}
