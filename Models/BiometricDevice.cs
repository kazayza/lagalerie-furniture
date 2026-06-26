using System;
using System.Collections.Generic;

namespace LagalerieFurniture.Models;

public partial class BiometricDevice
{
    public int Id { get; set; }

    public string DeviceName { get; set; } = null!;

    public string DeviceIp { get; set; } = null!;

    public int DevicePort { get; set; }

    public int BranchId { get; set; }

    public bool IsActive { get; set; }

    public DateTime? LastSyncAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Branch Branch { get; set; } = null!;

    public virtual ICollection<RawAttendanceLog> RawAttendanceLogs { get; set; } = new List<RawAttendanceLog>();
}
