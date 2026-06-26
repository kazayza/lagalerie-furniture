using System;
using System.Collections.Generic;

namespace LagalerieFurniture.Models;

public partial class VwBalanceSheet
{
    public string Category { get; set; } = null!;

    public string AccountName { get; set; } = null!;

    public string AccountCode { get; set; } = null!;

    public decimal? Balance { get; set; }
}
