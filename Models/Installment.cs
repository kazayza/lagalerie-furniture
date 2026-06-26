using System;
using System.Collections.Generic;

namespace LagalerieFurniture.Models;

public partial class Installment
{
    public long Id { get; set; }

    public long InvoiceId { get; set; }

    public long CustomerId { get; set; }

    public decimal TotalAmount { get; set; }

    public decimal DownPayment { get; set; }

    public int NumberOfInstallments { get; set; }

    public decimal InstallmentAmount { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public string Status { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual ICollection<InstallmentDetail> InstallmentDetails { get; set; } = new List<InstallmentDetail>();

    public virtual Invoice Invoice { get; set; } = null!;
}
