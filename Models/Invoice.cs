using System;
using System.Collections.Generic;

namespace LagalerieFurniture.Models;

public partial class Invoice
{
    public long Id { get; set; }

    public string InvoiceNumber { get; set; } = null!;

    public long? QuotationId { get; set; }

    public long CustomerId { get; set; }

    public DateTime InvoiceDate { get; set; }

    public DateTime DueDate { get; set; }

    public string InvoiceType { get; set; } = null!;

    public string Status { get; set; } = null!;

    public decimal SubTotal { get; set; }

    public decimal TaxAmount { get; set; }

    public decimal DiscountAmount { get; set; }

    public decimal TotalAmount { get; set; }

    public decimal PaidAmount { get; set; }

    public decimal RemainingAmount { get; set; }

    public string Currency { get; set; } = null!;

    public string? Notes { get; set; }

    public bool IsTaxInvoice { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedAt { get; set; }

    public int? DeletedById { get; set; }

    public int CreatedById { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<Complaint> Complaints { get; set; } = new List<Complaint>();

    public virtual User CreatedBy { get; set; } = null!;

    public virtual Customer Customer { get; set; } = null!;

    public virtual ICollection<Installment> Installments { get; set; } = new List<Installment>();

    public virtual ICollection<InvoiceItem> InvoiceItems { get; set; } = new List<InvoiceItem>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual Quotation? Quotation { get; set; }

    public virtual ICollection<Quotation> Quotations { get; set; } = new List<Quotation>();
}
