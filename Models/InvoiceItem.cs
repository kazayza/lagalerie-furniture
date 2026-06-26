using System;
using System.Collections.Generic;

namespace LagalerieFurniture.Models;

public partial class InvoiceItem
{
    public long Id { get; set; }

    public long InvoiceId { get; set; }

    public long? QuotationItemId { get; set; }

    public long? ProductVariantId { get; set; }

    public string Description { get; set; } = null!;

    public decimal Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal DiscountPercent { get; set; }

    public decimal TaxPercent { get; set; }

    public decimal LineTotal { get; set; }

    public bool IsBundle { get; set; }

    public long? BundleId { get; set; }

    public string? BundleComponents { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Product? Bundle { get; set; }

    public virtual Invoice Invoice { get; set; } = null!;

    public virtual ProductVariant? ProductVariant { get; set; }

    public virtual ICollection<ProductionOrder> ProductionOrders { get; set; } = new List<ProductionOrder>();

    public virtual QuotationItem? QuotationItem { get; set; }
}
