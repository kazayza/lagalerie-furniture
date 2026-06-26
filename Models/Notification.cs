using System;
using System.Collections.Generic;

namespace LagalerieFurniture.Models;

public partial class Notification
{
    public long Id { get; set; }

    public int UserId { get; set; }

    public string Title { get; set; } = null!;

    public string Message { get; set; } = null!;

    public string Type { get; set; } = null!;

    public string? Category { get; set; }

    public string? ReferenceType { get; set; }

    public long? ReferenceId { get; set; }

    public bool IsRead { get; set; }

    public DateTime? ReadAt { get; set; }

    public string? ActionUrl { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual User User { get; set; } = null!;
}
