using System;
using System.Collections.Generic;

namespace LagalerieFurniture.Models;

public partial class ProductImage
{
    public long Id { get; set; }

    public long ProductId { get; set; }

    public long? ProductVariantId { get; set; }

    public string ImageUrl { get; set; } = null!;

    public string? ThumbnailUrl { get; set; }

    public string? AltText { get; set; }

    public int SortOrder { get; set; }

    public bool IsPrimary { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual ProductVariant? ProductVariant { get; set; }
}
