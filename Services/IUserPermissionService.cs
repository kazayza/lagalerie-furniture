using LagalerieFurniture.Models;

namespace LagalerieFurniture.Services;

/// <summary>
/// إدارة الاستثناءات الشخصية للمستخدم (UserPermission) — grants/denies فوق الدور.
/// </summary>
public interface IUserPermissionService
{
    Task<List<UserPermissionDto>> GetUserPermissionsAsync(int userId);
    Task<List<PermissionMatrixItem>> GetUserPermissionMatrixAsync(int userId);
    /// <summary>يحفظ استثناءات المستخدم بالكامل (يستبدلها). كل عنصر = (كود، IsGranted, StartDate?, EndDate?, Reason?).</summary>
    Task<(bool Success, string? Error)> SetUserOverridesAsync(int userId, List<UserOverrideInput> overrides, int actedById);
}

public class UserPermissionDto
{
    public int Id { get; set; }
    public int PermissionId { get; set; }
    public string PermissionCode { get; set; } = null!;
    public string PermissionDisplayName { get; set; } = null!;
    public bool IsGranted { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Reason { get; set; }
}

/// <summary>عنصر في matrix الصلاحيات: حالة الصلاحية للمستخدم (من الدور + الاستثناء).</summary>
public class PermissionMatrixItem
{
    public int PermissionId { get; set; }
    public string Code { get; set; } = null!;
    public string DisplayName { get; set; } = null!;
    public int ModuleId { get; set; }
    public string ModuleDisplayName { get; set; } = null!;
    public string? Category { get; set; }
    /// <summary>من الدور.</summary>
    public bool FromRole { get; set; }
    /// <summary>استثناء شخصي موجود.</summary>
    public bool HasOverride { get; set; }
    /// <summary>النتيجة النهائية (effective).</summary>
    public bool Effective { get; set; }
    public DateTime? OverrideStartDate { get; set; }
    public DateTime? OverrideEndDate { get; set; }
}

public class UserOverrideInput
{
    public string PermissionCode { get; set; } = null!;
    public bool IsGranted { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Reason { get; set; }
}
