using System;
using System.Collections.Generic;

namespace LagalerieFurniture.Models;

public partial class Payment
{
    public long Id { get; set; }

    public string PaymentNumber { get; set; } = null!;

    public long? InvoiceId { get; set; }

    public long CustomerId { get; set; }

    public int? CashRegisterId { get; set; }

    public decimal Amount { get; set; }

    public string PaymentMethod { get; set; } = null!;

    public DateTime PaymentDate { get; set; }

    public string? ReferenceNumber { get; set; }

    public string? BankName { get; set; }

    public string? CheckNumber { get; set; }

    public DateTime? CheckDueDate { get; set; }

    public string Status { get; set; } = null!;

    public string? Notes { get; set; }

    public int ReceivedById { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual CashRegister? CashRegister { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual Invoice? Invoice { get; set; }

    public virtual User ReceivedBy { get; set; } = null!;
}
