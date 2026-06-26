using System;
using System.Collections.Generic;

namespace LagalerieFurniture.Models;

public partial class SystemSetting
{
    public int Id { get; set; }

    public string SettingKey { get; set; } = null!;

    public string SettingValue { get; set; } = null!;

    public string SettingType { get; set; } = null!;

    public string? Category { get; set; }

    public string? Description { get; set; }

    public bool IsEncrypted { get; set; }

    public int? UpdatedById { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual User? UpdatedBy { get; set; }
}
