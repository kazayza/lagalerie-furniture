using LagalerieFurniture.Data;
using LagalerieFurniture.Models;
using Microsoft.EntityFrameworkCore;

namespace LagalerieFurniture.Services;

public class UserPermissionService : IUserPermissionService
{
    private readonly ApplicationDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<UserPermissionService> _logger;

    public UserPermissionService(
        ApplicationDbContext context,
        IHttpContextAccessor httpContextAccessor,
        ILogger<UserPermissionService> logger)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<List<UserPermissionDto>> GetUserPermissionsAsync(int userId)
    {
        return await _context.UserPermissions
            .AsNoTracking()
            .Where(up => up.UserId == userId)
            .Select(up => new UserPermissionDto
            {
                Id = up.Id,
                PermissionId = up.PermissionId,
                PermissionCode = up.Permission.Code,
                PermissionDisplayName = up.Permission.DisplayName,
                IsGranted = up.IsGranted,
                StartDate = up.StartDate,
                EndDate = up.EndDate,
                Reason = up.Reason
            })
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<List<PermissionMatrixItem>> GetUserPermissionMatrixAsync(int userId)
    {
        var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null) return new List<PermissionMatrixItem>();

        // صلاحيات الدور
        var roleGranted = await _context.RolePermissions
            .AsNoTracking()
            .Where(rp => rp.RoleId == user.RoleId && rp.IsGranted)
            .Select(rp => rp.PermissionId)
            .ToListAsync();
        var roleSet = roleGranted.ToHashSet();

        // الاستثناءات الشخصية
        var overrides = await _context.UserPermissions
            .AsNoTracking()
            .Where(up => up.UserId == userId)
            .ToDictionaryAsync(up => up.PermissionId);

        var now = DateTime.UtcNow;

        // كل الصلاحيات مع موديولاتها
        var perms = await _context.Permissions
            .AsNoTracking()
            .OrderBy(p => p.Module.SortOrder).ThenBy(p => p.Category).ThenBy(p => p.Id)
            .Select(p => new
            {
                p.Id,
                p.Code,
                p.DisplayName,
                p.Category,
                ModuleId = p.ModuleId,
                ModuleDisplayName = p.Module.DisplayName
            })
            .ToListAsync();

        return perms.Select(p =>
        {
            var fromRole = roleSet.Contains(p.Id);
            overrides.TryGetValue(p.Id, out var ov);
            var effective = fromRole;

            if (ov != null)
            {
                var inRange = (ov.StartDate == null || ov.StartDate <= now)
                              && (ov.EndDate == null || ov.EndDate >= now);
                if (inRange)
                    effective = ov.IsGranted;
            }

            return new PermissionMatrixItem
            {
                PermissionId = p.Id,
                Code = p.Code,
                DisplayName = p.DisplayName,
                ModuleId = p.ModuleId,
                ModuleDisplayName = p.ModuleDisplayName,
                Category = p.Category,
                FromRole = fromRole,
                HasOverride = ov != null,
                Effective = effective,
                OverrideStartDate = ov?.StartDate,
                OverrideEndDate = ov?.EndDate
            };
        }).ToList();
    }

    /// <inheritdoc/>
    public async Task<(bool Success, string? Error)> SetUserOverridesAsync(int userId, List<UserOverrideInput> overrides, int actedById)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId && !u.IsDeleted);
        if (user == null)
            return (false, "المستخدم غير موجود");

        // خريطة كود ← id
        var codeToId = await _context.Permissions.AsNoTracking()
            .ToDictionaryAsync(p => p.Code, p => p.Id, StringComparer.OrdinalIgnoreCase);

        var existing = await _context.UserPermissions.Where(up => up.UserId == userId).ToListAsync();
        var existingByPermId = existing.ToDictionary(up => up.PermissionId);

        var now = DateTime.UtcNow;
        var desiredPermIds = new HashSet<int>();

        foreach (var inp in overrides)
        {
            if (!codeToId.TryGetValue(inp.PermissionCode, out var permId))
                continue;
            desiredPermIds.Add(permId);

            if (existingByPermId.TryGetValue(permId, out var row))
            {
                row.IsGranted = inp.IsGranted;
                row.StartDate = inp.StartDate;
                row.EndDate = inp.EndDate;
                row.Reason = inp.Reason;
                row.GrantedById = actedById;
                row.GrantedAt = now;
            }
            else
            {
                _context.UserPermissions.Add(new UserPermission
                {
                    UserId = userId,
                    PermissionId = permId,
                    IsGranted = inp.IsGranted,
                    StartDate = inp.StartDate,
                    EndDate = inp.EndDate,
                    Reason = inp.Reason,
                    GrantedById = actedById,
                    GrantedAt = now
                });
            }
        }

        // احذف أي استثناءات مكنتش في القائمة الجديدة
        var toRemove = existing.Where(up => !desiredPermIds.Contains(up.PermissionId)).ToList();
        foreach (var up in toRemove)
            _context.UserPermissions.Remove(up);

        await _context.SaveChangesAsync();

        // سجل تدقيق
        try
        {
            _context.PermissionAuditLogs.Add(new PermissionAuditLog
            {
                UserId = actedById,
                TargetUserId = userId,
                Action = "user.set_overrides",
                NewValue = $"{overrides.Count} استثناء شخصي",
                IpAddress = _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString(),
                CreatedAt = now
            });
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "فشل تسجيل العملية في سجل التدقيق");
        }

        _logger.LogInformation("تم تحديث استثناءات المستخدم {UserId}: {Count} استثناء بواسطة {Actor}",
            userId, overrides.Count, actedById);
        return (true, null);
    }
}
