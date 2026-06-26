using System;
using System.Collections.Generic;

namespace LagalerieFurniture.Models;

public partial class AttributeValue
{
    public int Id { get; set; }
    public int AttributeId { get; set; }
    public string Value { get; set; } = null!;
    public string? ColorCode { get; set; }
    public int DisplayOrder { get; set; }

    public virtual ProductAttribute Attribute { get; set; } = null!;
    public virtual ICollection<ProductVariantAttribute> ProductVariantAttributes { get; set; } = new List<ProductVariantAttribute>();
}