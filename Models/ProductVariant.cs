using System;
using System.Collections.Generic;

namespace LagalerieFurniture.Models;

public partial class ProductVariant
{
    public long Id { get; set; }

    public long ProductId { get; set; }

    public string Sku { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Barcode { get; set; }

    public decimal CostPrice { get; set; }

    public decimal? ActualCost { get; set; }

    public decimal WholesalePrice { get; set; }

    public decimal RetailPrice { get; set; }

    public decimal? ShowroomPrice { get; set; }

    public decimal? DiscountPrice { get; set; }

    public DateTime? DiscountStart { get; set; }

    public DateTime? DiscountEnd { get; set; }

    public int StockQuantity { get; set; }

    public int MinStockLevel { get; set; }

    public int? MaxStockLevel { get; set; }

    public string? ImageUrl { get; set; }

    public bool IsActive { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedAt { get; set; }

    public int? DeletedById { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<GoodsReceiptItem> GoodsReceiptItems { get; set; } = new List<GoodsReceiptItem>();

    public virtual ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();

    public virtual ICollection<InventoryTransaction> InventoryTransactions { get; set; } = new List<InventoryTransaction>();

    public virtual ICollection<InventoryTransferItem> InventoryTransferItems { get; set; } = new List<InventoryTransferItem>();

    public virtual ICollection<InvoiceItem> InvoiceItems { get; set; } = new List<InvoiceItem>();

    public virtual ICollection<MaterialConsumption> MaterialConsumptions { get; set; } = new List<MaterialConsumption>();

    public virtual Product Product { get; set; } = null!;

    public virtual ICollection<ProductBundleComponent> ProductBundleComponents { get; set; } = new List<ProductBundleComponent>();

    public virtual ICollection<ProductImage> ProductImages { get; set; } = new List<ProductImage>();

    public virtual ICollection<ProductVariantAttribute> ProductVariantAttributes { get; set; } = new List<ProductVariantAttribute>();

    public virtual ICollection<PurchaseOrderItem> PurchaseOrderItems { get; set; } = new List<PurchaseOrderItem>();

    public virtual ICollection<QuotationItem> QuotationItems { get; set; } = new List<QuotationItem>();

    public virtual ICollection<StockCountAdjustment> StockCountAdjustments { get; set; } = new List<StockCountAdjustment>();

    public virtual ICollection<StockCountItem> StockCountItems { get; set; } = new List<StockCountItem>();
}
