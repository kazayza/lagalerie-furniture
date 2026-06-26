using System;
using System.Collections.Generic;

namespace LagalerieFurniture.Models;

public partial class ActivityLog
{
    public long Id { get; set; }

    public int? UserId { get; set; }

    public string ActivityType { get; set; } = null!;

    public string? EntityType { get; set; }

    public long? EntityId { get; set; }

    public string? Description { get; set; }

    public string? OldData { get; set; }

    public string? NewData { get; set; }

    public string? IpAddress { get; set; }

    public string? UserAgent { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual User? User { get; set; }
}
