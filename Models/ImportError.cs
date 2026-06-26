using System;
using System.Collections.Generic;

namespace LagalerieFurniture.Models;

public partial class ImportError
{
    public long Id { get; set; }

    public long ImportId { get; set; }

    public int RowNumber { get; set; }

    public string ColumnName { get; set; } = null!;

    public string ErrorMessage { get; set; } = null!;

    public string? RawValue { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual DataImportsExport Import { get; set; } = null!;
}
