using LagalerieFurniture.Models;

namespace LagalerieFurniture.Services;

/// <summary>
/// نظام حساب الصلاحيات الفعّالة لكل مستخدم.
/// القاعدة: نبدأ بصلاحيات الدور (RolePermission) ثم نطبّق الاستثناءات الشخصية (UserPermission).
/// الـ User override يلغي أو يمنح صلاحية محددة فوق الدور.
/// </summary>
public interface IPermissionService
{
    /// <summary>
    /// يحسب كل أكواد الصلاحيات الفعّالة للمستخدم (Role + User overrides ضمن نطاق التاريخ).
    /// </summary>
    Task<HashSet<string>> GetEffectivePermissionCodesAsync(int userId);

    /// <summary>
    /// فحص سريع لصلاحية واحدة.
    /// </summary>
    Task<bool> HasPermissionAsync(int userId, string permissionCode);

    /// <summary>
    /// يرجّع الموديولات اللي يقدر المستخدم يفتحها (للقائمة الجانبية الديناميكية).
    /// الموديول يظهر لو المستخدم عنده أي صلاحية .view فيه.
    /// </summary>
    Task<List<NavigationModuleDto>> GetAccessibleModulesAsync(int userId);

    /// <summary>
    /// كل الصلاحيات المتاحة في النظام (لشاشة matrix الأدوار).
    /// </summary>
    Task<List<Permission>> GetAllPermissionsAsync();

    /// <summary>
    /// كل الموديولات المفعّلة (للعرض في القائمة ومصفوفة الصلاحيات).
    /// </summary>
    Task<List<Module>> GetAllModulesAsync();
}

/// <summary>
/// موديول قابل للعرض في القائمة الجانبية مع أبنائه.
/// </summary>
public class NavigationModuleDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string DisplayName { get; set; } = null!;
    public string? Icon { get; set; }
    public int SortOrder { get; set; }
    public List<NavigationLinkDto> Links { get; set; } = new();
}

/// <summary>
/// عنصر قائمة داخل موديول.
/// </summary>
public class NavigationLinkDto
{
    public string Label { get; set; } = null!;
    public string Href { get; set; } = null!;
    public string? Icon { get; set; }
    public string? PermissionCode { get; set; }
}
