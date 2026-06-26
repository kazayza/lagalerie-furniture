using System;
using System.Collections.Generic;

namespace LagalerieFurniture.Models;

public partial class RawAttendanceLog
{
    public long Id { get; set; }

    public int DeviceId { get; set; }

    public int EmployeeId { get; set; }

    public DateTime LogDateTime { get; set; }

    public string LogType { get; set; } = null!;

    public string? VerifiedBy { get; set; }

    public bool IsProcessed { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual BiometricDevice Device { get; set; } = null!;

    public virtual Employee Employee { get; set; } = null!;
}
