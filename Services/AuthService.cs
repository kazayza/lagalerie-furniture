using System.Security.Claims;
using LagalerieFurniture.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

namespace LagalerieFurniture.Services;

public class AuthService : IAuthService
{
    private readonly ApplicationDbContext _context;
    private readonly CustomAuthStateProvider _authStateProvider;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<AuthService> _logger;

    // إعدادات قفل الحساب بعد محاولات فاشلة
    private const int MaxFailedAttempts = 5;
    private static readonly TimeSpan LockoutDuration = TimeSpan.FromMinutes(15);

    public AuthService(
        ApplicationDbContext context,
        CustomAuthStateProvider authStateProvider,
        IPasswordHasher passwordHasher,
        IHttpContextAccessor httpContextAccessor,
        ILogger<AuthService> logger)
    {
        _context = context;
        _authStateProvider = authStateProvider;
        _passwordHasher = passwordHasher;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    public async Task<(bool Success, string? ErrorMessage, string? Username, string? Role)> LoginAsync(string username, string password)
    {
        try
        {
            // تنظيف اسم المستخدم من المسافات الزائدة
            username = username?.Trim() ?? string.Empty;

            _logger.LogInformation("محاولة تسجيل دخول للمستخدم: {Username}", username);

            // نحتاج تتبّع الكيان (tracking) عشان نحدّث LastLoginAt / FailedAttempts
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username);

            // فحص 1: المستخدم موجود؟ (نفس الرسالة عشان ما نكشفش وجود الحساب)
            if (user == null)
            {
                _logger.LogWarning("المستخدم غير موجود: {Username}", username);
                return (false, "اسم المستخدم أو كلمة المرور غير صحيحة", null, null);
            }

            // فحص 2: الحساب محذوف؟
            if (user.IsDeleted)
            {
                _logger.LogWarning("الحساب محذوف: {Username}", username);
                return (false, "هذا الحساب محذوف", null, null);
            }

            // فحص 3: الحساب نشط؟
            if (!user.IsActive)
            {
                _logger.LogWarning("الحساب غير نشط: {Username}", username);
                return (false, "هذا الحساب معطّل، يرجى الاتصال بالمدير", null, null);
            }

            // فحص 4: الحساب مقفل مؤقتاً (بعد محاولات فاشلة)؟
            if (user.LockoutEnd.HasValue && user.LockoutEnd.Value > DateTime.UtcNow)
            {
                var remaining = user.LockoutEnd.Value - DateTime.UtcNow;
                _logger.LogWarning("الحساب مقفل: {Username} لمدة {Minutes} دقيقة", username, Math.Ceiling(remaining.TotalMinutes));
                return (false, $"تم قفل الحساب مؤقتاً، حاول بعد {Math.Ceiling(remaining.TotalMinutes)} دقيقة", null, null);
            }

            // فحص 5: كلمة المرور (BCrypt مع دعم الباسوردات القديمة النصية)
            var passwordValid = VerifyPassword(user.PasswordHash, password);

            if (!passwordValid)
            {
                // زيادة عدّاد المحاولات الفاشلة
                user.FailedLoginAttempts++;
                if (user.FailedLoginAttempts >= MaxFailedAttempts)
                {
                    user.LockoutEnd = DateTime.UtcNow.Add(LockoutDuration);
                    _logger.LogWarning("تم قفل الحساب بعد {Count} محاولات فاشلة: {Username}", MaxFailedAttempts, username);
                }
                await _context.SaveChangesAsync();

                _logger.LogWarning("كلمة مرور خاطئة للمستخدم: {Username} (محاولة {Count})", username, user.FailedLoginAttempts);
                return (false, "اسم المستخدم أو كلمة المرور غير صحيحة", null, null);
            }

            // ✅ نجاح! تصفير عدّاد المحاولات الفاشلة وتسجيل بيانات الدخول
            user.FailedLoginAttempts = 0;
            user.LockoutEnd = null;
            user.LastLoginAt = DateTime.UtcNow;
            user.LastLoginIp = GetClientIp();

            await _context.SaveChangesAsync();

            // ترقية كلمات المرور القديمة (نص صريح) إلى BCrypt بعد أول دخول ناجح
            if (!_passwordHasher.IsBCryptHash(user.PasswordHash))
            {
                var rehashed = user.PasswordHash; // نحتفظ بالقيمة قبل التغيير
                user.PasswordHash = _passwordHasher.Hash(password);
                await _context.SaveChangesAsync();
                _logger.LogInformation("تمت ترقية تشفير كلمة مرور المستخدم تلقائياً: {Username}", username);
            }

            // جلب اسم الدور
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

            // إصدار Authentication Cookie حقيقية (تعيش بعد الـ refresh وإعادة التشغيل)
            await SignInWithCookieAsync(user, roleName);

            // تحديث حالة المصادقة في Blazor circuit
            _authStateProvider.MarkUserAsAuthenticated(user.Username, roleName);

            _logger.LogInformation("تسجيل دخول ناجح: {Username} (دور: {Role})", username, roleName);

            return (true, null, user.Username, roleName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "خطأ أثناء تسجيل الدخول للمستخدم: {Username}", username);
            return (false, $"حدث خطأ في الاتصال: {ex.Message}", null, null);
        }
    }

    /// <summary>
    /// التحقق من كلمة المرور مع دعم الحالة الانتقالية:
    /// - لو الـ hash المخزّن BCrypt صالح → Verify
    /// - لو نص صريح قديم → مقارنة مباشرة + re-hash تلقائي للترقية
    /// </summary>
    private bool VerifyPassword(string storedPassword, string providedPassword)
    {
        if (_passwordHasher.IsBCryptHash(storedPassword))
        {
            return _passwordHasher.Verify(providedPassword, storedPassword);
        }

        // حساب قديم بكلمة مرور نصية — نطابق ثم نرقّي لـ BCrypt
        if (storedPassword == providedPassword)
        {
            _logger.LogWarning("باسورد قديم غير مشفّر — سيتم ترقيتها تلقائياً بعد الدخول");
            // الترقية تتم بشكل منفصل (RehashLegacyPasswordAsync) بعد تأكيد النجاح
            return true;
        }

        return false;
    }

    /// <summary>
    /// إصدار Authentication Cookie يحوي الـ claims الأساسية.
    /// </summary>
    private async Task SignInWithCookieAsync(Models.User user, string roleName)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext == null)
        {
            _logger.LogWarning("HttpContext غير متوفر — لا يمكن إصدار cookie. سيتم الاعتماد على حالة الـ circuit فقط.");
            return;
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.Username),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.GivenName, user.DisplayName),
            new(ClaimTypes.Role, roleName),
        };

        var identity = new ClaimsIdentity(claims, "LagalerieCookie");
        var principal = new ClaimsPrincipal(identity);

        await httpContext.SignInAsync(
            "LagalerieCookie",
            principal,
            new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7),
                AllowRefresh = true
            });
    }

    public async Task LogoutAsync()
    {
        // مسح الـ cookie أولاً
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext != null)
        {
            await httpContext.SignOutAsync("LagalerieCookie");
        }

        _authStateProvider.MarkUserAsLoggedOut();
    }

    public Task<string?> GetCurrentUserAsync()
    {
        var state = _authStateProvider.GetAuthenticationStateAsync().Result;
        var username = state.User.Identity?.Name;
        return Task.FromResult(username);
    }

    private string? GetClientIp()
    {
        return _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();
    }
}
