using LagalerieFurniture.Data;
using Microsoft.EntityFrameworkCore;

namespace LagalerieFurniture.Services;

public class AuthService : IAuthService
{
    private readonly ApplicationDbContext _context;
    private readonly CustomAuthStateProvider _authStateProvider;
    private readonly ILogger<AuthService> _logger;

    public AuthService(
        ApplicationDbContext context,
        CustomAuthStateProvider authStateProvider,
        ILogger<AuthService> logger)
    {
        _context = context;
        _authStateProvider = authStateProvider;
        _logger = logger;
    }

    public async Task<(bool Success, string? ErrorMessage, string? Username, string? Role)> LoginAsync(string username, string password)
    {
        try
        {
            // تنظيف اسم المستخدم من المسافات الزائدة
            username = username?.Trim() ?? string.Empty;

            _logger.LogInformation("محاولة تسجيل دخول للمستخدم: {Username}", username);

            // البحث عن المستخدم بدون Include عشان نتجنب مشاكل العلاقات
            var user = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Username == username);

            // فحص 1: المستخدم موجود؟
            if (user == null)
            {
                _logger.LogWarning("المستخدم غير موجود: {Username}", username);
                return (false, "اسم المستخدم أو كلمة المرور غير صحيحة", null, null);
            }

            // فحص 2: الحساب نشط؟
            if (!user.IsActive)
            {
                _logger.LogWarning("الحساب غير نشط: {Username}", username);
                return (false, "هذا الحساب معطّل، يرجى الاتصال بالمدير", null, null);
            }

            // فحص 3: الحساب محذوف؟
            if (user.IsDeleted)
            {
                _logger.LogWarning("الحساب محذوف: {Username}", username);
                return (false, "هذا الحساب محذوف", null, null);
            }

            // فحص 4: كلمة المرور
            if (user.PasswordHash != password)
            {
                _logger.LogWarning("كلمة مرور خاطئة للمستخدم: {Username}", username);
                return (false, "اسم المستخدم أو كلمة المرور غير صحيحة", null, null);
            }

            // نجاح! تحديث حالة المصادقة
            _logger.LogInformation("تسجيل دخول ناجح: {Username}", username);

            var roleName = "User";
            try
            {
                var role = await _context.Roles
                    .AsNoTracking()
                    .FirstOrDefaultAsync(r => r.Id == user.RoleId);
                if (role != null)
                    roleName = role.Name ?? "User";
            }
            catch
            {
                // لو فيه مشكلة في الـ Role، نكمل بدونها
            }

            // تحديث حالة المصادقة في Blazor
            _authStateProvider.MarkUserAsAuthenticated(user.Username, roleName);

            return (true, null, user.Username, roleName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "خطأ أثناء تسجيل الدخول للمستخدم: {Username}", username);
            return (false, $"حدث خطأ في الاتصال: {ex.Message}", null, null);
        }
    }

    public Task LogoutAsync()
    {
        _authStateProvider.MarkUserAsLoggedOut();
        return Task.CompletedTask;
    }

    public Task<string?> GetCurrentUserAsync()
    {
        var state = _authStateProvider.GetAuthenticationStateAsync().Result;
        var username = state.User.Identity?.Name;
        return Task.FromResult(username);
    }
}
