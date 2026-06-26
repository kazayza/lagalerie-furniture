using System;
using System.Collections.Generic;

namespace LagalerieFurniture.Models;

public partial class Product
{
    public long Id { get; set; }

    public int CategoryId { get; set; }

    public string Sku { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? NameEn { get; set; }

    public string? Description { get; set; }

    public string? ShortDescription { get; set; }

    public string? Specifications { get; set; }

    public string? ImageUrl { get; set; }

    public string ProductType { get; set; } = null!;

    public bool IsBundle { get; set; }

    public decimal? BundleDiscount { get; set; }

    public bool IsCustomizable { get; set; }

    public bool IsManufactured { get; set; }

    public decimal? Weight { get; set; }

    public int DefaultWarrantyMonths { get; set; }

    public bool IsActive { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedAt { get; set; }

    public int? DeletedById { get; set; }

    public int? BranchId { get; set; }

    public int? CreatedById { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual BillOfMaterial? BillOfMaterial { get; set; }

    public virtual ICollection<Bomitem> Bomitems { get; set; } = new List<Bomitem>();

    public virtual Branch? Branch { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<Complaint> Complaints { get; set; } = new List<Complaint>();

    public virtual User? CreatedBy { get; set; }

    public virtual ICollection<GoodsReceiptItem> GoodsReceiptItems { get; set; } = new List<GoodsReceiptItem>();

    public virtual ICollection<InvoiceItem> InvoiceItems { get; set; } = new List<InvoiceItem>();

    public virtual ICollection<ProductBundleComponent> ProductBundleComponentBundleProducts { get; set; } = new List<ProductBundleComponent>();

    public virtual ICollection<ProductBundleComponent> ProductBundleComponentComponentProducts { get; set; } = new List<ProductBundleComponent>();

    public virtual ICollection<ProductImage> ProductImages { get; set; } = new List<ProductImage>();

    public virtual ICollection<ProductVariant> ProductVariants { get; set; } = new List<ProductVariant>();

    public virtual ICollection<ProductionOrder> ProductionOrders { get; set; } = new List<ProductionOrder>();

    public virtual ICollection<PurchaseOrderItem> PurchaseOrderItems { get; set; } = new List<PurchaseOrderItem>();

    public virtual ICollection<QuotationItem> QuotationItems { get; set; } = new List<QuotationItem>();

    public virtual ICollection<StockCountItem> StockCountItems { get; set; } = new List<StockCountItem>();
}
