using System;
using System.Collections.Generic;

namespace LagalerieFurniture.Models;

public partial class DataImportsExport
{
    public long Id { get; set; }

    public string OperationType { get; set; } = null!;

    public string EntityType { get; set; } = null!;

    public string FileName { get; set; } = null!;

    public string FilePath { get; set; } = null!;

    public string FileFormat { get; set; } = null!;

    public int TotalRecords { get; set; }

    public int ProcessedRecords { get; set; }

    public int FailedRecords { get; set; }

    public string Status { get; set; } = null!;

    public string? ErrorLog { get; set; }

    public int StartedById { get; set; }

    public DateTime StartedAt { get; set; }

    public DateTime? CompletedAt { get; set; }

    public string? Notes { get; set; }

    public virtual ICollection<ImportError> ImportErrors { get; set; } = new List<ImportError>();

    public virtual User StartedBy { get; set; } = null!;
}
