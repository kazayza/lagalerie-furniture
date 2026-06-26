using System;
using System.Collections.Generic;

namespace LagalerieFurniture.Models;

public partial class ProductBundleComponent
{
    public long Id { get; set; }

    public long BundleProductId { get; set; }

    public long ComponentProductId { get; set; }

    public long? ComponentVariantId { get; set; }

    public int Quantity { get; set; }

    public bool IsOptional { get; set; }

    public int SortOrder { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Product BundleProduct { get; set; } = null!;

    public virtual Product ComponentProduct { get; set; } = null!;

    public virtual ProductVariant? ComponentVariant { get; set; }
}
