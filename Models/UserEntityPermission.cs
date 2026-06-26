using System;
using System.Collections.Generic;

namespace LagalerieFurniture.Models;

public partial class UserEntityPermission
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string EntityType { get; set; } = null!;

    public long EntityId { get; set; }

    public string PermissionType { get; set; } = null!;

    public int? GrantedById { get; set; }

    public DateTime GrantedAt { get; set; }
}
