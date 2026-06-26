using System;
using System.Collections.Generic;

namespace LagalerieFurniture.Models;

public partial class Customer
{
    public long Id { get; set; }

    public string CustomerCode { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string DisplayName { get; set; } = null!;

    public string? Email { get; set; }

    public string Phone { get; set; } = null!;

    public string? Phone2 { get; set; }

    public string? Address { get; set; }

    public string? City { get; set; }

    public string? Area { get; set; }

    public string? Notes { get; set; }

    public int? LeadSourceId { get; set; }

    public string CustomerType { get; set; } = null!;

    public decimal CreditLimit { get; set; }

    public decimal CurrentBalance { get; set; }

    public decimal TotalPurchases { get; set; }

    public int TotalOrders { get; set; }

    public DateTime? LastPurchaseDate { get; set; }

    public int LoyaltyPoints { get; set; }

    public int? AssignedToId { get; set; }

    public bool IsBlacklisted { get; set; }

    public string? BlacklistReason { get; set; }

    public bool IsActive { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedAt { get; set; }

    public int? DeletedById { get; set; }

    public int? BranchId { get; set; }

    public int? CreatedById { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual User? AssignedTo { get; set; }

    public virtual Branch? Branch { get; set; }

    public virtual ICollection<Complaint> Complaints { get; set; } = new List<Complaint>();

    public virtual User? CreatedBy { get; set; }

    public virtual ICollection<Installment> Installments { get; set; } = new List<Installment>();

    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

    public virtual LeadSource? LeadSource { get; set; }

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual ICollection<Quotation> Quotations { get; set; } = new List<Quotation>();
}
