using System;
using System.Collections.Generic;

namespace LagalerieFurniture.Models;

public partial class QuotationItem
{
    public long Id { get; set; }

    public long QuotationId { get; set; }

    public long? ProductId { get; set; }

    public long? ProductVariantId { get; set; }

    public string ItemDescription { get; set; } = null!;

    public decimal Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal DiscountPercent { get; set; }

    public decimal TaxPercent { get; set; }

    public decimal LineTotal { get; set; }

    public string? Notes { get; set; }

    public int SortOrder { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<InvoiceItem> InvoiceItems { get; set; } = new List<InvoiceItem>();

    public virtual Product? Product { get; set; }

    public virtual ProductVariant? ProductVariant { get; set; }

    public virtual Quotation Quotation { get; set; } = null!;
}
