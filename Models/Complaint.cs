using System;
using System.Collections.Generic;

namespace LagalerieFurniture.Models;

public partial class Complaint
{
    public long Id { get; set; }

    public string ComplaintNumber { get; set; } = null!;

    public long CustomerId { get; set; }

    public long? InvoiceId { get; set; }

    public long? ProductId { get; set; }

    public string Type { get; set; } = null!;

    public string Priority { get; set; } = null!;

    public string Status { get; set; } = null!;

    public string Subject { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string? Resolution { get; set; }

    public int? AssignedToId { get; set; }

    public DateTime OpenedAt { get; set; }

    public DateTime? FirstResponseAt { get; set; }

    public DateTime? ResolvedAt { get; set; }

    public DateTime? ClosedAt { get; set; }

    public int? SatisfactionRating { get; set; }

    public int CreatedById { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual User? AssignedTo { get; set; }

    public virtual ICollection<ComplaintActivity> ComplaintActivities { get; set; } = new List<ComplaintActivity>();

    public virtual User CreatedBy { get; set; } = null!;

    public virtual Customer Customer { get; set; } = null!;

    public virtual Invoice? Invoice { get; set; }

    public virtual Product? Product { get; set; }
}
