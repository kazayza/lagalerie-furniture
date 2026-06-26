using System;
using System.Collections.Generic;

namespace LagalerieFurniture.Models;

public partial class VwDailyAttendance
{
    public string EmployeeName { get; set; } = null!;

    public string EmployeeCode { get; set; } = null!;

    public DateOnly Date { get; set; }

    public DateTime? CheckIn { get; set; }

    public DateTime? CheckOut { get; set; }

    public decimal WorkingHours { get; set; }

    public decimal OvertimeHours { get; set; }

    public string Status { get; set; } = null!;

    public string Department { get; set; } = null!;
}
