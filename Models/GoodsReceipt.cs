using System;
using System.Collections.Generic;

namespace LagalerieFurniture.Models;

public partial class GoodsReceipt
{
    public long Id { get; set; }

    public string ReceiptNumber { get; set; } = null!;

    public long PurchaseOrderId { get; set; }

    public int SupplierId { get; set; }

    public int? WarehouseId { get; set; }

    public DateTime ReceiptDate { get; set; }

    public string? DocumentNumber { get; set; }

    public string? DriverName { get; set; }

    public string? VehicleNumber { get; set; }

    public string? Notes { get; set; }

    public string Status { get; set; } = null!;

    public int? InspectedById { get; set; }

    public DateTime? InspectedAt { get; set; }

    public int CreatedById { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual User CreatedBy { get; set; } = null!;

    public virtual ICollection<GoodsReceiptItem> GoodsReceiptItems { get; set; } = new List<GoodsReceiptItem>();

    public virtual User? InspectedBy { get; set; }

    public virtual PurchaseOrder PurchaseOrder { get; set; } = null!;

    public virtual Supplier Supplier { get; set; } = null!;

    public virtual Warehouse? Warehouse { get; set; }
}
