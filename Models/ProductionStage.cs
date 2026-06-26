using System;
using System.Collections.Generic;

namespace LagalerieFurniture.Models;

public partial class ProductionStage
{
    public long Id { get; set; }

    public long ProductionOrderId { get; set; }

    public string StageName { get; set; } = null!;

    public int StageOrder { get; set; }

    public string Status { get; set; } = null!;

    public int? AssignedToId { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public decimal? EstimatedHours { get; set; }

    public decimal? ActualHours { get; set; }

    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual User? AssignedTo { get; set; }

    public virtual ICollection<MaterialConsumption> MaterialConsumptions { get; set; } = new List<MaterialConsumption>();

    public virtual ProductionOrder ProductionOrder { get; set; } = null!;
}
