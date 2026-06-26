using System;
using System.Collections.Generic;

namespace LagalerieFurniture.Models;

public partial class PermissionAuditLog
{
    public long Id { get; set; }

    public int UserId { get; set; }

    public int? TargetUserId { get; set; }

    public int? TargetRoleId { get; set; }

    public string Action { get; set; } = null!;

    public int? PermissionId { get; set; }

    public string? EntityType { get; set; }

    public long? EntityId { get; set; }

    public string? OldValue { get; set; }

    public string? NewValue { get; set; }

    public string? IpAddress { get; set; }

    public DateTime CreatedAt { get; set; }
}
