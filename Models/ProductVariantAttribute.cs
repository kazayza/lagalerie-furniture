using System;

namespace LagalerieFurniture.Models;

public partial class ProductVariantAttribute
{
    public int Id { get; set; }
    public long ProductVariantId { get; set; }
    public int AttributeId { get; set; }
    public int AttributeValueId { get; set; }

    public virtual ProductAttribute Attribute { get; set; } = null!;
    public virtual AttributeValue AttributeValue { get; set; } = null!;
    public virtual ProductVariant ProductVariant { get; set; } = null!;
}