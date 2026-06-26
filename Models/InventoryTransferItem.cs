using System;
using System.Collections.Generic;

namespace LagalerieFurniture.Models;

public partial class InventoryTransferItem
{
    public long Id { get; set; }

    public long TransferId { get; set; }

    public long ProductVariantId { get; set; }

    public decimal Quantity { get; set; }

    public decimal ReceivedQuantity { get; set; }

    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ProductVariant ProductVariant { get; set; } = null!;

    public virtual InventoryTransfer Transfer { get; set; } = null!;
}
