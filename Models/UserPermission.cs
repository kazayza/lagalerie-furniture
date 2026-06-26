using System;
using System.Collections.Generic;

namespace LagalerieFurniture.Models;

public partial class UserPermission
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int PermissionId { get; set; }

    public bool IsGranted { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public string? Reason { get; set; }

    public int? GrantedById { get; set; }

    public DateTime GrantedAt { get; set; }

    public virtual Permission Permission { get; set; } = null!;
}
