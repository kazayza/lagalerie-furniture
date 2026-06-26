using System;
using System.Collections.Generic;

namespace LagalerieFurniture.Models;

public partial class ComplaintActivity
{
    public long Id { get; set; }

    public long ComplaintId { get; set; }

    public string ActivityType { get; set; } = null!;

    public string Description { get; set; } = null!;

    public bool IsInternal { get; set; }

    public int CreatedById { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Complaint Complaint { get; set; } = null!;

    public virtual User CreatedBy { get; set; } = null!;
}
