using System;
using System.Collections.Generic;

namespace LagalerieFurniture.Models;

public partial class VwInventoryStatus
{
    public string ProductName { get; set; } = null!;

    public string VariantSku { get; set; } = null!;

    public string VariantName { get; set; } = null!;

    public string WarehouseName { get; set; } = null!;

    public string BranchName { get; set; } = null!;

    public decimal QuantityOnHand { get; set; }

    public decimal QuantityReserved { get; set; }

    public decimal QuantityAvailable { get; set; }

    public int ReorderLevel { get; set; }

    public string StockStatus { get; set; } = null!;
}
