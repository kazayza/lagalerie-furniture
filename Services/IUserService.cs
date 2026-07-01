using LagalerieFurniture.Models;

namespace LagalerieFurniture.Services;

/// <summary>
/// إدارة المستخدمين: إنشاء/تعديل/حذف مبدئي + تفعيل/قفل/إعادة تعيين كلمة المرور.
/// كل العمليات تُسجَّل في PermissionAuditLog.
/// </summary>
public interface IUserService
{
    Task<PagedResult<UserListItemDto>> GetUsersAsync(UserFilter filter);
    Task<User?> GetByIdAsync(int id);
    Task<(bool Success, string? Error, int? UserId)> CreateAsync(CreateUserDto dto, int createdById);
    Task<(bool Success, string? Error)> UpdateAsync(UpdateUserDto dto, int updatedById);
    Task<(bool Success, string? Error)> DeleteAsync(int userId, int deletedById);
    Task<(bool Success, string? Error)> ToggleActiveAsync(int userId, int actedById);
    Task<(bool Success, string? Error)> UnlockAsync(int userId, int actedById);
    Task<(bool Success, string? Error)> ResetPasswordAsync(int userId, string newPassword, int actedById);
    Task<(bool Success, string? Error)> ChangeOwnPasswordAsync(int userId, string currentPassword, string newPassword);
    Task<List<Role>> GetActiveRolesAsync();
    Task<List<Branch>> GetActiveBranchesAsync();
}

/// <summary>نتيجة بصفحات.</summary>
public class PagedResult<T>
{
    public List<T> Items { get; set; } = new();
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => PageSize == 0 ? 0 : (int)Math.Ceiling(TotalCount / (double)PageSize);
}

/// <summary>فلتر للبحث.</summary>
public class UserFilter
{
    public string? Search { get; set; }
    public int? RoleId { get; set; }
    public bool? IsActive { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}

/// <summary>صف صف مستخدم في القائمة.</summary>
public class UserListItemDto
{
    public int Id { get; set; }
    public string Username { get; set; } = null!;
    public string DisplayName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? AvatarUrl { get; set; }
    public string RoleName { get; set; } = null!;
    public string? RoleDisplayName { get; set; }
    public bool IsActive { get; set; }
    public bool IsLocked => LockoutEnd.HasValue && LockoutEnd.Value > DateTime.UtcNow;
    public DateTime? LockoutEnd { get; set; }
    public DateTime? LastLoginAt { get; set; }
    public DateTime CreatedAt { get; set; }
}

/// <summary>بيانات إنشاء مستخدم.</summary>
public class CreateUserDto
{
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? Phone { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Password { get; set; } = null!;
    public int RoleId { get; set; }
    public int? DefaultBranchId { get; set; }
    public bool IsActive { get; set; } = true;
    public bool MustChangePassword { get; set; }
    public string? AvatarUrl { get; set; }
}

/// <summary>بيانات تعديل مستخدم.</summary>
public class UpdateUserDto
{
    public int Id { get; set; }
    public string Email { get; set; } = null!;
    public string? Phone { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public int RoleId { get; set; }
    public int? DefaultBranchId { get; set; }
    public string? AvatarUrl { get; set; }
}
