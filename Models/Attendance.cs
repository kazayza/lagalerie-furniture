using System;
using System.Collections.Generic;

namespace LagalerieFurniture.Models;

public partial class Attendance
{
    public long Id { get; set; }

    public int EmployeeId { get; set; }

    public DateOnly Date { get; set; }

    public DateTime? CheckIn { get; set; }

    public DateTime? CheckOut { get; set; }

    public DateTime? BreakStart { get; set; }

    public DateTime? BreakEnd { get; set; }

    public decimal WorkingHours { get; set; }

    public decimal OvertimeHours { get; set; }

    public int LateMinutes { get; set; }

    public int EarlyLeaveMinutes { get; set; }

    public int MissedPunches { get; set; }

    public string Status { get; set; } = null!;

    public int? ShiftScheduleId { get; set; }

    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Employee Employee { get; set; } = null!;

    public virtual ShiftSchedule? ShiftSchedule { get; set; }
}
