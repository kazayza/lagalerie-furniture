using LagalerieFurniture.Data;
using LagalerieFurniture.Models;
using Microsoft.EntityFrameworkCore;

namespace LagalerieFurniture.Services;

/// <summary>
/// تنفيذ إدارة المستخدمين.
/// يستخدم IDbContextFactory عشان كل عملية DB تـ create DbContext مستقل —
/// ده ضروري في Blazor Server عشان نتجنب concurrent access على نفس الـ DbContext.
/// كل عمليات التعديل تُسجَّل في PermissionAuditLog.
/// </summary>
public class UserService : IUserService
{
    private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<UserService> _logger;

    public UserService(
        IDbContextFactory<ApplicationDbContext> contextFactory,
        IPasswordHasher passwordHasher,
        IHttpContextAccessor httpContextAccessor,
        ILogger<UserService> logger)
    {
        _contextFactory = contextFactory;
        _passwordHasher = passwordHasher;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<PagedResult<UserListItemDto>> GetUsersAsync(UserFilter filter)
    {
        // نتأكد إن Page وPageSize موجبين (defensive — قدام أي مكان يبعت قيم 0 أو سالبة)
        var page = filter.Page < 1 ? 1 : filter.Page;
        var pageSize = filter.PageSize < 1 ? 20 : filter.PageSize;

        using var context = _contextFactory.CreateDbContext();
        var query = context.Users
            .AsNoTracking()
            .Where(u => !u.IsDeleted);

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var s = filter.Search.Trim();
            query = query.Where(u =>
                u.Username.Contains(s) ||
                u.Email.Contains(s) ||
                (u.FirstName + " " + u.LastName).Contains(s) ||
                u.DisplayName.Contains(s));
        }

        if (filter.RoleId.HasValue)
            query = query.Where(u => u.RoleId == filter.RoleId.Value);

        if (filter.IsActive.HasValue)
            query = query.Where(u => u.IsActive == filter.IsActive.Value);

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderByDescending(u => u.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(u => new UserListItemDto
            {
                Id = u.Id,
                Username = u.Username,
                DisplayName = u.DisplayName,
                Email = u.Email,
                AvatarUrl = u.AvatarUrl,
                RoleName = u.Role.Name,
                RoleDisplayName = u.Role.DisplayName,
                IsActive = u.IsActive,
                LockoutEnd = u.LockoutEnd,
                LastLoginAt = u.LastLoginAt,
                CreatedAt = u.CreatedAt
            })
            .ToListAsync();

        return new PagedResult<UserListItemDto>
        {
            Items = items,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }

    /// <inheritdoc/>
    public async Task<User?> GetByIdAsync(int id)
    {
        using var context = _contextFactory.CreateDbContext();
        return await context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted);
    }

    /// <inheritdoc/>
    public async Task<(bool Success, string? Error, int? UserId)> CreateAsync(CreateUserDto dto, int createdById)
    {
        using var context = _contextFactory.CreateDbContext();

        // التحقق من عدم تكرار اسم المستخدم/الإيميل
        var exists = await context.Users.AnyAsync(u =>
            !u.IsDeleted && (u.Username == dto.Username || u.Email == dto.Email));
        if (exists)
            return (false, "اسم المستخدم أو البريد الإلكتروني مستخدم بالفعل", null);

        // التحقق من وجود الدور
        var roleExists = await context.Roles.AnyAsync(r => r.Id == dto.RoleId && !r.IsDeleted);
        if (!roleExists)
            return (false, "الدور المحدد غير موجود", null);

        var now = DateTime.UtcNow;
        var user = new User
        {
            Username = dto.Username.Trim(),
            Email = dto.Email.Trim(),
            Phone = dto.Phone,
            FirstName = dto.FirstName.Trim(),
            LastName = dto.LastName.Trim(),
            DisplayName = $"{dto.FirstName.Trim()} {dto.LastName.Trim()}",
            PasswordHash = _passwordHasher.Hash(dto.Password),
            RoleId = dto.RoleId,
            DefaultBranchId = dto.DefaultBranchId,
            IsActive = dto.IsActive,
            MustChangePassword = dto.MustChangePassword,
            CreatedAt = now,
            FailedLoginAttempts = 0
        };

        context.Users.Add(user);
        await context.SaveChangesAsync();

        await LogAuditAsync(createdById, targetUserId: user.Id, action: "user.create",
            newValue: $"username={user.Username}, role={dto.RoleId}");

        _logger.LogInformation("تم إنشاء مستخدم جديد: {Username} (id={Id}) بواسطة {Actor}", user.Username, user.Id, createdById);
        return (true, null, user.Id);
    }

    /// <inheritdoc/>
    public async Task<(bool Success, string? Error)> UpdateAsync(UpdateUserDto dto, int updatedById)
    {
        using var context = _contextFactory.CreateDbContext();
        var user = await context.Users.FirstOrDefaultAsync(u => u.Id == dto.Id && !u.IsDeleted);
        if (user == null)
            return (false, "المستخدم غير موجود");

        // التحقق من عدم تكرار الإيميل لمستخدم آخر
        var emailTaken = await context.Users.AnyAsync(u =>
            u.Id != dto.Id && !u.IsDeleted && u.Email == dto.Email);
        if (emailTaken)
            return (false, "البريد الإلكتروني مستخدم من قبل مستخدم آخر");

        var oldValues = $"email={user.Email}, role={user.RoleId}";

        user.Email = dto.Email.Trim();
        user.Phone = dto.Phone;
        user.FirstName = dto.FirstName.Trim();
        user.LastName = dto.LastName.Trim();
        user.DisplayName = $"{user.FirstName} {user.LastName}";
        user.RoleId = dto.RoleId;
        user.DefaultBranchId = dto.DefaultBranchId;
        user.UpdatedAt = DateTime.UtcNow;

        await context.SaveChangesAsync();

        var newValues = $"email={user.Email}, role={user.RoleId}";
        await LogAuditAsync(updatedById, targetUserId: user.Id, action: "user.update",
            oldValue: oldValues, newValue: newValues);

        _logger.LogInformation("تم تعديل المستخدم: {Id} بواسطة {Actor}", dto.Id, updatedById);
        return (true, null);
    }

    /// <inheritdoc/>
    public async Task<(bool Success, string? Error)> DeleteAsync(int userId, int deletedById)
    {
        using var context = _contextFactory.CreateDbContext();
        var user = await context.Users.FirstOrDefaultAsync(u => u.Id == userId && !u.IsDeleted);
        if (user == null)
            return (false, "المستخدم غير موجود");

        // منع حذف نفسك
        if (userId == deletedById)
            return (false, "لا يمكنك حذف حسابك الخاص");

        // soft delete
        var now = DateTime.UtcNow;
        user.IsDeleted = true;
        user.DeletedAt = now;
        user.DeletedById = deletedById;
        user.IsActive = false;
        user.UpdatedAt = now;

        await context.SaveChangesAsync();

        await LogAuditAsync(deletedById, targetUserId: userId, action: "user.delete",
            oldValue: "active", newValue: "deleted");

        _logger.LogInformation("تم حذف المستخدم: {Id} بواسطة {Actor}", userId, deletedById);
        return (true, null);
    }

    /// <inheritdoc/>
    public async Task<(bool Success, string? Error)> ToggleActiveAsync(int userId, int actedById)
    {
        using var context = _contextFactory.CreateDbContext();
        var user = await context.Users.FirstOrDefaultAsync(u => u.Id == userId && !u.IsDeleted);
        if (user == null)
            return (false, "المستخدم غير موجود");

        user.IsActive = !user.IsActive;
        user.UpdatedAt = DateTime.UtcNow;
        await context.SaveChangesAsync();

        await LogAuditAsync(actedById, targetUserId: userId, action: "user.toggle_active",
            oldValue: (!user.IsActive).ToString(), newValue: user.IsActive.ToString());

        _logger.LogInformation("تبديل تفعيل المستخدم {Id}: {State} بواسطة {Actor}", userId, user.IsActive, actedById);
        return (true, null);
    }

    /// <inheritdoc/>
    public async Task<(bool Success, string? Error)> UnlockAsync(int userId, int actedById)
    {
        using var context = _contextFactory.CreateDbContext();
        var user = await context.Users.FirstOrDefaultAsync(u => u.Id == userId && !u.IsDeleted);
        if (user == null)
            return (false, "المستخدم غير موجود");

        user.LockoutEnd = null;
        user.FailedLoginAttempts = 0;
        user.UpdatedAt = DateTime.UtcNow;
        await context.SaveChangesAsync();

        await LogAuditAsync(actedById, targetUserId: userId, action: "user.unlock");

        _logger.LogInformation("تم فك قفل المستخدم {Id} بواسطة {Actor}", userId, actedById);
        return (true, null);
    }

    /// <inheritdoc/>
    public async Task<(bool Success, string? Error)> ResetPasswordAsync(int userId, string newPassword, int actedById)
    {
        if (string.IsNullOrWhiteSpace(newPassword) || newPassword.Length < 6)
            return (false, "كلمة المرور يجب أن تكون 6 أحرف على الأقل");

        using var context = _contextFactory.CreateDbContext();
        var user = await context.Users.FirstOrDefaultAsync(u => u.Id == userId && !u.IsDeleted);
        if (user == null)
            return (false, "المستخدم غير موجود");

        user.PasswordHash = _passwordHasher.Hash(newPassword);
        user.MustChangePassword = true;
        user.LockoutEnd = null;
        user.FailedLoginAttempts = 0;
        user.UpdatedAt = DateTime.UtcNow;
        await context.SaveChangesAsync();

        await LogAuditAsync(actedById, targetUserId: userId, action: "user.reset_password");

        _logger.LogInformation("تمت إعادة تعيين كلمة مرور المستخدم {Id} بواسطة {Actor}", userId, actedById);
        return (true, null);
    }

    /// <inheritdoc/>
    public async Task<(bool Success, string? Error)> ChangeOwnPasswordAsync(int userId, string currentPassword, string newPassword)
    {
        if (string.IsNullOrWhiteSpace(newPassword) || newPassword.Length < 6)
            return (false, "كلمة المرور الجديدة يجب أن تكون 6 أحرف على الأقل");

        using var context = _contextFactory.CreateDbContext();
        var user = await context.Users.FirstOrDefaultAsync(u => u.Id == userId && !u.IsDeleted);
        if (user == null)
            return (false, "المستخدم غير موجود");

        // التحقق من كلمة المرور الحالية
        if (!_passwordHasher.Verify(currentPassword, user.PasswordHash))
            return (false, "كلمة المرور الحالية غير صحيحة");

        user.PasswordHash = _passwordHasher.Hash(newPassword);
        user.MustChangePassword = false;
        user.UpdatedAt = DateTime.UtcNow;
        await context.SaveChangesAsync();

        await LogAuditAsync(userId, targetUserId: userId, action: "user.change_password_self");

        _logger.LogInformation("غيّر المستخدم {Id} كلمة مروره", userId);
        return (true, null);
    }

    /// <inheritdoc/>
    public async Task<List<Role>> GetActiveRolesAsync()
    {
        using var context = _contextFactory.CreateDbContext();
        return await context.Roles.AsNoTracking().Where(r => !r.IsDeleted && r.IsActive).OrderBy(r => r.DisplayName).ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<List<Branch>> GetActiveBranchesAsync()
    {
        using var context = _contextFactory.CreateDbContext();
        return await context.Branches.AsNoTracking().OrderBy(b => b.Name).ToListAsync();
    }

    /// <summary>تسجيل عملية في سجل تدقيق الصلاحيات.</summary>
    private async Task LogAuditAsync(
        int actorUserId,
        string action,
        int? targetUserId = null,
        int? targetRoleId = null,
        int? permissionId = null,
        string? oldValue = null,
        string? newValue = null)
    {
        try
        {
            using var context = _contextFactory.CreateDbContext();
            context.PermissionAuditLogs.Add(new PermissionAuditLog
            {
                UserId = actorUserId,
                TargetUserId = targetUserId,
                TargetRoleId = targetRoleId,
                Action = action,
                PermissionId = permissionId,
                OldValue = oldValue,
                NewValue = newValue,
                IpAddress = GetClientIp(),
                CreatedAt = DateTime.UtcNow
            });
            await context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            // السجل تكميلي — ما يفشلش العملية الأساسية
            _logger.LogError(ex, "فشل تسجيل العملية في سجل التدقيق: {Action}", action);
        }
    }

    private string? GetClientIp() =>
        _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();
}