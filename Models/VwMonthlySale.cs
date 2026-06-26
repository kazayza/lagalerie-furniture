using System;
using System.Collections.Generic;

namespace LagalerieFurniture.Models;

public partial class VwMonthlySale
{
    public int? Year { get; set; }

    public int? Month { get; set; }

    public int? TotalInvoices { get; set; }

    public decimal? TotalRevenue { get; set; }

    public decimal? TotalPaid { get; set; }

    public decimal? AvgInvoiceValue { get; set; }
}
