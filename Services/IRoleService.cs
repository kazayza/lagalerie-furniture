using LagalerieFurniture.Models;

namespace LagalerieFurniture.Services;

/// <summary>
/// إدارة الأدوار وصلاحياتها (RolePermission).
/// أدوار الـ system (IsSystem) لا تُحذف ولا يُغيَّر اسمها.
/// </summary>
public interface IRoleService
{
    Task<List<Role>> GetAllAsync();
    Task<Role?> GetByIdAsync(int id);
    Task<RoleDetailsDto?> GetDetailsAsync(int id);
    Task<(bool Success, string? Error, int? RoleId)> CreateAsync(CreateRoleDto dto, int createdById);
    Task<(bool Success, string? Error)> UpdateAsync(UpdateRoleDto dto, int updatedById);
    Task<(bool Success, string? Error)> DeleteAsync(int roleId, int deletedById);
    /// <summary>يحفظ صلاحيات الدور بالكامل (يستبدلها). قائمة أكواد الصلاحيات الممنوحة.</summary>
    Task<(bool Success, string? Error)> SetPermissionsAsync(int roleId, HashSet<string> grantedCodes, int actedById);
}

public class RoleDetailsDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string DisplayName { get; set; } = null!;
    public string? Description { get; set; }
    public bool IsSystem { get; set; }
    public bool IsActive { get; set; }
    public int UsersCount { get; set; }
    public HashSet<string> GrantedPermissionCodes { get; set; } = new(StringComparer.OrdinalIgnoreCase);
}

public class CreateRoleDto
{
    public string Name { get; set; } = null!;
    public string DisplayName { get; set; } = null!;
    public string? Description { get; set; }
}

public class UpdateRoleDto
{
    public int Id { get; set; }
    public string DisplayName { get; set; } = null!;
    public string? Description { get; set; }
    public bool IsActive { get; set; }
}
