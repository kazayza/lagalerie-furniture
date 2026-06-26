using System;
using System.Collections.Generic;

namespace LagalerieFurniture.Models;

public partial class ProductAttribute
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string DisplayName { get; set; } = null!;
    public string Type { get; set; } = null!;
    public bool IsRequired { get; set; }
    public int SortOrder { get; set; }
    public DateTime CreatedAt { get; set; }

    // ============================================================
    // Navigation Properties (العلاقات)
    // ============================================================
    public virtual ICollection<AttributeValue> AttributeValues { get; set; } = new List<AttributeValue>();
    public virtual ICollection<ProductVariantAttribute> ProductVariantAttributes { get; set; } = new List<ProductVariantAttribute>();
}