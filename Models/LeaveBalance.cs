using System;
using System.Collections.Generic;

namespace LagalerieFurniture.Models;

public partial class LeaveBalance
{
    public long Id { get; set; }

    public int EmployeeId { get; set; }

    public int Year { get; set; }

    public string LeaveType { get; set; } = null!;

    public int TotalDays { get; set; }

    public int UsedDays { get; set; }

    public int RemainingDays { get; set; }

    public int CarriedOver { get; set; }

    public virtual Employee Employee { get; set; } = null!;
}
