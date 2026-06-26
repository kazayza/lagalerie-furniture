using System;
using System.Collections.Generic;

namespace LagalerieFurniture.Models;

public partial class FixedAsset
{
    public long Id { get; set; }

    public string AssetCode { get; set; } = null!;

    public string AssetName { get; set; } = null!;

    public string Category { get; set; } = null!;

    public string? SerialNumber { get; set; }

    public DateOnly PurchaseDate { get; set; }

    public decimal PurchaseCost { get; set; }

    public decimal? SalvageValue { get; set; }

    public int UsefulLifeYears { get; set; }

    public decimal DepreciationRate { get; set; }

    public string DepreciationMethod { get; set; } = null!;

    public decimal CurrentValue { get; set; }

    public decimal AccumulatedDepreciation { get; set; }

    public int? LocationId { get; set; }

    public int BranchId { get; set; }

    public string Status { get; set; } = null!;

    public bool IsDeleted { get; set; }

    public DateTime? DeletedAt { get; set; }

    public int? DeletedById { get; set; }

    public string? Notes { get; set; }

    public int CreatedById { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<AssetDepreciation> AssetDepreciations { get; set; } = new List<AssetDepreciation>();

    public virtual ICollection<AssetTransaction> AssetTransactions { get; set; } = new List<AssetTransaction>();

    public virtual Branch Branch { get; set; } = null!;

    public virtual User CreatedBy { get; set; } = null!;

    public virtual Warehouse? Location { get; set; }
}
