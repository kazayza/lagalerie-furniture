using System;
using System.Collections.Generic;

namespace LagalerieFurniture.Models;

public partial class Supplier
{
    public int Id { get; set; }

    public string SupplierCode { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? NameEn { get; set; }

    public string? ContactPerson { get; set; }

    public string Phone { get; set; } = null!;

    public string? Phone2 { get; set; }

    public string? Email { get; set; }

    public string? Address { get; set; }

    public string? City { get; set; }

    public string? TaxNumber { get; set; }

    public string? CommercialRegister { get; set; }

    public string? PaymentTerms { get; set; }

    public string Currency { get; set; } = null!;

    public decimal CreditLimit { get; set; }

    public decimal CurrentBalance { get; set; }

    public decimal? Rating { get; set; }

    public int? LeadTimeDays { get; set; }

    public bool IsActive { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedAt { get; set; }

    public int? DeletedById { get; set; }

    public string? Notes { get; set; }

    public int? CreatedById { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual User? CreatedBy { get; set; }

    public virtual ICollection<GoodsReceipt> GoodsReceipts { get; set; } = new List<GoodsReceipt>();

    public virtual ICollection<PurchaseOrder> PurchaseOrders { get; set; } = new List<PurchaseOrder>();
}
