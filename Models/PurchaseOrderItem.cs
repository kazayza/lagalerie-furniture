using System;
using System.Collections.Generic;

namespace LagalerieFurniture.Models;

public partial class PurchaseOrderItem
{
    public long Id { get; set; }

    public long PurchaseOrderId { get; set; }

    public long? ProductId { get; set; }

    public long? ProductVariantId { get; set; }

    public string ItemDescription { get; set; } = null!;

    public decimal Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public string? UnitMeasure { get; set; }

    public decimal DiscountPercent { get; set; }

    public decimal TaxPercent { get; set; }

    public decimal LineTotal { get; set; }

    public decimal ReceivedQuantity { get; set; }

    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<GoodsReceiptItem> GoodsReceiptItems { get; set; } = new List<GoodsReceiptItem>();

    public virtual Product? Product { get; set; }

    public virtual ProductVariant? ProductVariant { get; set; }

    public virtual PurchaseOrder PurchaseOrder { get; set; } = null!;
}
