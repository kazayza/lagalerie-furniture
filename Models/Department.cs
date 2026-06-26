using System;
using System.Collections.Generic;

namespace LagalerieFurniture.Models;

public partial class Department
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string DisplayName { get; set; } = null!;

    public int? ParentDepartmentId { get; set; }

    public int? ManagerId { get; set; }

    public string? Description { get; set; }

    public bool IsActive { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedAt { get; set; }

    public int? DeletedById { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<CostCenter> CostCenters { get; set; } = new List<CostCenter>();

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

    public virtual ICollection<Department> InverseParentDepartment { get; set; } = new List<Department>();

    public virtual User? Manager { get; set; }

    public virtual Department? ParentDepartment { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
