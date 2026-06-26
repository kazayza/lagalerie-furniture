using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;

namespace LagalerieFurniture.Services;

/// <summary>
/// مزوّد حالة المصادقة لـ Blazor.
/// يقرأ بيانات المستخدم من الـ Authentication Cookie (عبر HttpContext)
/// بدلاً من الذاكرة المؤقتة فقط — فينتج عنه:
///   - استمرار الجلسة بعد عمل Refresh (F5)
///   - استمرار الجلسة بعد إعادة تشغيل السيرفر
///   - أمان حقيقي على مستوى الـ HTTP request
/// </summary>
public class CustomAuthStateProvider : AuthenticationStateProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<CustomAuthStateProvider> _logger;

    public CustomAuthStateProvider(
        IHttpContextAccessor httpContextAccessor,
        ILogger<CustomAuthStateProvider> logger)
    {
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var httpContextUser = _httpContextAccessor.HttpContext?.User;

        // لو فيه مستخدم فعلي من الـ cookie، نستخدمه
        if (httpContextUser?.Identity?.IsAuthenticated == true)
        {
            return Task.FromResult(new AuthenticationState(httpContextUser));
        }

        // anonymous
        var anonymous = new ClaimsPrincipal(new ClaimsIdentity());
        return Task.FromResult(new AuthenticationState(anonymous));
    }

    /// <summary>
    /// إشعار Blazor بتحديث حالة المصادقة بعد تسجيل الدخول/الخروج.
    /// ملاحظة: في وضع Cookie، البيانات الفعلية بتتحط عن طريق HttpContext.SignInAsync
    /// في صفحة الـ Login، وهذه الدالة فقط تُحدّث واجهة Blazor (circuit).
    /// </summary>
    public void MarkUserAsAuthenticated(string username, string role = "User")
    {
        var identity = new ClaimsIdentity(
            new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, role),
            },
            "LagalerieCookie");

        var principal = new ClaimsPrincipal(identity);
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(principal)));
    }

    public void MarkUserAsLoggedOut()
    {
        var anonymous = new ClaimsPrincipal(new ClaimsIdentity());
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(anonymous)));
    }
}
