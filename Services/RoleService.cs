using LagalerieFurniture.Data;
using LagalerieFurniture.Models;
using Microsoft.EntityFrameworkCore;

namespace LagalerieFurniture.Services;

public class RoleService : IRoleService
{
    private readonly ApplicationDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<RoleService> _logger;

    public RoleService(
        ApplicationDbContext context,
        IHttpContextAccessor httpContextAccessor,
        ILogger<RoleService> logger)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    /// <inheritdoc/>
    public Task<List<Role>> GetAllAsync() =>
        _context.Roles.AsNoTracking().Where(r => !r.IsDeleted).OrderBy(r => r.DisplayName).ToListAsync();

    /// <inheritdoc/>
    public Task<Role?> GetByIdAsync(int id) =>
        _context.Roles.AsNoTracking().FirstOrDefaultAsync(r => r.Id == id && !r.IsDeleted);

    /// <inheritdoc/>
    public async Task<RoleDetailsDto?> GetDetailsAsync(int id)
    {
        var role = await _context.Roles
            .AsNoTracking()
            .Where(r => r.Id == id && !r.IsDeleted)
            .Select(r => new RoleDetailsDto
            {
                Id = r.Id,
                Name = r.Name,
                DisplayName = r.DisplayName,
                Description = r.Description,
                IsSystem = r.IsSystem,
                IsActive = r.IsActive,
                UsersCount = r.Users.Count(u => !u.IsDeleted)
            })
            .FirstOrDefaultAsync();

        if (role == null) return null;

        var codes = await _context.RolePermissions
            .AsNoTracking()
            .Where(rp => rp.RoleId == id && rp.IsGranted)
            .Select(rp => rp.Permission.Code)
            .ToListAsync();

        role.GrantedPermissionCodes = new HashSet<string>(codes, StringComparer.OrdinalIgnoreCase);
        return role;
    }

    /// <inheritdoc/>
    public async Task<(bool Success, string? Error, int? RoleId)> CreateAsync(CreateRoleDto dto, int createdById)
    {
        var nameTaken = await _context.Roles.AnyAsync(r => !r.IsDeleted && r.Name == dto.Name.Trim());
        if (nameTaken)
            return (false, "اسم الدور مستخدم بالفعل", null);

        var now = DateTime.UtcNow;
        var role = new Role
        {
            Name = dto.Name.Trim(),
            DisplayName = dto.DisplayName.Trim(),
            Description = dto.Description,
            IsSystem = false,
            IsActive = true,
            CreatedAt = now
        };

        _context.Roles.Add(role);
        await _context.SaveChangesAsync();

        await LogAuditAsync(createdById, action: "role.create", targetRoleId: role.Id,
            newValue: $"name={role.Name}");
        _logger.LogInformation("تم إنشاء دور: {Name} (id={Id}) بواسطة {Actor}", role.Name, role.Id, createdById);
        return (true, null, role.Id);
    }

    /// <inheritdoc/>
    public async Task<(bool Success, string? Error)> UpdateAsync(UpdateRoleDto dto, int updatedById)
    {
        var role = await _context.Roles.FirstOrDefaultAsync(r => r.Id == dto.Id && !r.IsDeleted);
        if (role == null)
            return (false, "الدور غير موجود");

        var oldValues = $"displayName={role.DisplayName}, active={role.IsActive}";

        role.DisplayName = dto.DisplayName.Trim();
        role.Description = dto.Description;
        role.IsActive = dto.IsActive;
        role.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        var newValues = $"displayName={role.DisplayName}, active={role.IsActive}";
        await LogAuditAsync(updatedById, action: "role.update", targetRoleId: role.Id,
            oldValue: oldValues, newValue: newValues);
        _logger.LogInformation("تم تعديل الدور {Id} بواسطة {Actor}", dto.Id, updatedById);
        return (true, null);
    }

    /// <inheritdoc/>
    public async Task<(bool Success, string? Error)> DeleteAsync(int roleId, int deletedById)
    {
        var role = await _context.Roles.FirstOrDefaultAsync(r => r.Id == roleId && !r.IsDeleted);
        if (role == null)
            return (false, "الدور غير موجود");

        if (role.IsSystem)
            return (false, "لا يمكن حذف أدوار النظام الأساسية");

        // منع الحذف لو في مستخدمون تابعون
        var hasUsers = await _context.Users.AnyAsync(u => u.RoleId == roleId && !u.IsDeleted);
        if (hasUsers)
            return (false, "لا يمكن حذف الدور لوجود مستخدمين تابعين له. غيّر أدوارهم أولاً");

        var now = DateTime.UtcNow;
        role.IsDeleted = true;
        role.DeletedAt = now;
        role.DeletedById = deletedById;
        role.IsActive = false;
        role.UpdatedAt = now;

        await _context.SaveChangesAsync();

        await LogAuditAsync(deletedById, action: "role.delete", targetRoleId: roleId,
            oldValue: "active", newValue: "deleted");
        _logger.LogInformation("تم حذف الدور {Id} بواسطة {Actor}", roleId, deletedById);
        return (true, null);
    }

    /// <inheritdoc/>
    public async Task<(bool Success, string? Error)> SetPermissionsAsync(int roleId, HashSet<string> grantedCodes, int actedById)
    {
        var role = await _context.Roles.FirstOrDefaultAsync(r => r.Id == roleId && !r.IsDeleted);
        if (role == null)
            return (false, "الدور غير موجود");

        // كل الصلاحيات الحالية للدور
        var existing = await _context.RolePermissions.Where(rp => rp.RoleId == roleId).ToListAsync();
        // كل الأكواد ← ID
        var allPerms = await _context.Permissions.AsNoTracking().ToListAsync();
        var codeToId = allPerms.ToDictionary(p => p.Code, p => p.Id, StringComparer.OrdinalIgnoreCase);

        var now = DateTime.UtcNow;
        var newCodesStr = string.Join(",", grantedCodes.OrderBy(c => c));

        foreach (var rp in existing)
        {
            var code = allPerms.First(p => p.Id == rp.PermissionId).Code;
            var shouldGrant = grantedCodes.Contains(code);
            if (rp.IsGranted != shouldGrant)
            {
                rp.IsGranted = shouldGrant;
                rp.GrantedById = actedById;
                rp.GrantedAt = now;
            }
        }

        // أضف أي صلاحيات جديدة مكنتش موجودة أصلاً
        var existingCodes = existing.Select(rp => allPerms.First(p => p.Id == rp.PermissionId).Code).ToHashSet(StringComparer.OrdinalIgnoreCase);
        foreach (var code in grantedCodes)
        {
            if (existingCodes.Contains(code)) continue;
            if (!codeToId.TryGetValue(code, out var permId)) continue;

            _context.RolePermissions.Add(new RolePermission
            {
                RoleId = roleId,
                PermissionId = permId,
                IsGranted = true,
                GrantedById = actedById,
                GrantedAt = now
            });
        }

        await _context.SaveChangesAsync();

        await LogAuditAsync(actedById, action: "role.set_permissions", targetRoleId: roleId,
            newValue: newCodesStr);
        _logger.LogInformation("تم تحديث صلاحيات الدور {Id}: {Count} صلاحية ممنوحة بواسطة {Actor}",
            roleId, grantedCodes.Count, actedById);
        return (true, null);
    }

    private async Task LogAuditAsync(
        int actorUserId,
        string action,
        int? targetRoleId = null,
        int? targetUserId = null,
        int? permissionId = null,
        string? oldValue = null,
        string? newValue = null)
    {
        try
        {
            _context.PermissionAuditLogs.Add(new PermissionAuditLog
            {
                UserId = actorUserId,
                TargetUserId = targetUserId,
                TargetRoleId = targetRoleId,
                Action = action,
                PermissionId = permissionId,
                OldValue = oldValue,
                NewValue = newValue,
                IpAddress = _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString(),
                CreatedAt = DateTime.UtcNow
            });
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "فشل تسجيل العملية في سجل التدقيق: {Action}", action);
        }
    }
}
