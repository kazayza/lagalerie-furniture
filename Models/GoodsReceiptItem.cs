using System;
using System.Collections.Generic;

namespace LagalerieFurniture.Models;

public partial class GoodsReceiptItem
{
    public long Id { get; set; }

    public long GoodsReceiptId { get; set; }

    public long PurchaseOrderItemId { get; set; }

    public long? ProductId { get; set; }

    public long? ProductVariantId { get; set; }

    public decimal QuantityOrdered { get; set; }

    public decimal QuantityReceived { get; set; }

    public decimal QuantityDamaged { get; set; }

    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual GoodsReceipt GoodsReceipt { get; set; } = null!;

    public virtual Product? Product { get; set; }

    public virtual ProductVariant? ProductVariant { get; set; }

    public virtual PurchaseOrderItem PurchaseOrderItem { get; set; } = null!;
}
