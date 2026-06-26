using System;
using System.Collections.Generic;

namespace LagalerieFurniture.Models;

public partial class Quotation
{
    public long Id { get; set; }

    public string QuotationNumber { get; set; } = null!;

    public long CustomerId { get; set; }

    public long? LeadId { get; set; }

    public DateTime ValidUntil { get; set; }

    public string Status { get; set; } = null!;

    public decimal SubTotal { get; set; }

    public decimal TaxAmount { get; set; }

    public decimal DiscountAmount { get; set; }

    public decimal TotalAmount { get; set; }

    public string? Notes { get; set; }

    public string? TermsConditions { get; set; }

    public int CreatedById { get; set; }

    public long? ConvertedInvoiceId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Invoice? ConvertedInvoice { get; set; }

    public virtual User CreatedBy { get; set; } = null!;

    public virtual Customer Customer { get; set; } = null!;

    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

    public virtual ICollection<QuotationItem> QuotationItems { get; set; } = new List<QuotationItem>();
}
