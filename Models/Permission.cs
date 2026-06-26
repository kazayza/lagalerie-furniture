using System;
using System.Collections.Generic;

namespace LagalerieFurniture.Models;

public partial class Permission
{
    public int Id { get; set; }

    public int ModuleId { get; set; }

    public string Name { get; set; } = null!;

    public string DisplayName { get; set; } = null!;

    public string? Description { get; set; }

    public string Code { get; set; } = null!;

    public string? Category { get; set; }

    public bool IsSystem { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Module Module { get; set; } = null!;

    public virtual ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();

    public virtual ICollection<UserPermission> UserPermissions { get; set; } = new List<UserPermission>();
}
