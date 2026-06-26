using System;
using System.Collections.Generic;

namespace LagalerieFurniture.Models;

public partial class VwDueInstallment
{
    public long Id { get; set; }

    public string CustomerName { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public int InstallmentNumber { get; set; }

    public DateTime DueDate { get; set; }

    public decimal Amount { get; set; }

    public decimal PaidAmount { get; set; }

    public string Status { get; set; } = null!;

    public long InvoiceId { get; set; }

    public int? DaysUntilDue { get; set; }
}
