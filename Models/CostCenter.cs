using System;
using System.Collections.Generic;

namespace LagalerieFurniture.Models;

public partial class CostCenter
{
    public int Id { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public int? ParentCostCenterId { get; set; }

    public int? DepartmentId { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Department? Department { get; set; }

    public virtual ICollection<CostCenter> InverseParentCostCenter { get; set; } = new List<CostCenter>();

    public virtual ICollection<JournalEntryLine> JournalEntryLines { get; set; } = new List<JournalEntryLine>();

    public virtual CostCenter? ParentCostCenter { get; set; }
}
