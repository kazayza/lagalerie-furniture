using System;
using System.Collections.Generic;

namespace LagalerieFurniture.Models;

public partial class Module
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string DisplayName { get; set; } = null!;

    public string? Description { get; set; }

    public string? Icon { get; set; }

    public int SortOrder { get; set; }

    public bool IsSystem { get; set; }

    public bool IsEnabled { get; set; }

    public int? RequiresModuleId { get; set; }

    public string? Version { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<Module> InverseRequiresModule { get; set; } = new List<Module>();

    public virtual ICollection<Permission> Permissions { get; set; } = new List<Permission>();

    public virtual Module? RequiresModule { get; set; }
}
