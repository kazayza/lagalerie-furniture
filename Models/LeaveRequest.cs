using System;
using System.Collections.Generic;

namespace LagalerieFurniture.Models;

public partial class LeaveRequest
{
    public long Id { get; set; }

    public int EmployeeId { get; set; }

    public string LeaveType { get; set; } = null!;

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public int TotalDays { get; set; }

    public string? Reason { get; set; }

    public string Status { get; set; } = null!;

    public int? ApprovedById { get; set; }

    public DateTime? ApprovedAt { get; set; }

    public string? RejectionReason { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual User? ApprovedBy { get; set; }

    public virtual Employee Employee { get; set; } = null!;
}
