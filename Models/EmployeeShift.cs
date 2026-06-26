using System;
using System.Collections.Generic;

namespace LagalerieFurniture.Models;

public partial class EmployeeShift
{
    public long Id { get; set; }

    public int EmployeeId { get; set; }

    public int ShiftId { get; set; }

    public DateOnly EffectiveDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public bool IsDefault { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Employee Employee { get; set; } = null!;

    public virtual ShiftSchedule Shift { get; set; } = null!;
}
