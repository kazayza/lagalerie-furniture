using System;
using System.Collections.Generic;

namespace LagalerieFurniture.Models;

public partial class VwEmployeeAdvance
{
    public string AdvanceNumber { get; set; } = null!;

    public string EmployeeName { get; set; } = null!;

    public decimal Amount { get; set; }

    public int InstallmentCount { get; set; }

    public decimal InstallmentAmount { get; set; }

    public string Status { get; set; } = null!;

    public DateTime RequestDate { get; set; }

    public decimal TotalDeducted { get; set; }

    public decimal? RemainingBalance { get; set; }
}
