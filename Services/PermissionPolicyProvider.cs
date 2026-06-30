using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace LagalerieFurniture.Services;

/// <summary>
/// يوفّر policies بشكل ديناميكي: أي اسم policy مساوي لكود صلاحية (مثل "users.view")
/// يتحوّل تلقائياً لـ policy تتطلب claim "permission" بنفس القيمة.
/// كده مفيش حاجة نسجّل كل policy يدوياً — أي كود صلاحية يشتغل فوراً مع [Authorize(Policy="...")].
/// </summary>
public class PermissionPolicyProvider : IAuthorizationPolicyProvider
{
    private readonly DefaultAuthorizationPolicyProvider _fallbackProvider;

    public PermissionPolicyProvider(IOptions<AuthorizationOptions> options)
    {
        _fallbackProvider = new DefaultAuthorizationPolicyProvider(options);
    }

    public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
        => _fallbackProvider.GetDefaultPolicyAsync();

    public Task<AuthorizationPolicy?> GetFallbackPolicyAsync()
        => _fallbackProvider.GetFallbackPolicyAsync();

    public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        // أي policy اسمها كود صلاحية (يحتوي على "_" أو "." أو يطابق نمط UPPER_SNAKE)
        // → نحوّلها لمتطلب claim "permission"
        // أمثلة: "users.view", "DASH_VIEW", "users_create", "SET_VIEW"
        if (policyName.Contains('.') || policyName.Contains('_'))
        {
            var policy = new AuthorizationPolicyBuilder()
                .RequireClaim("permission", policyName)
                .Build();
            return Task.FromResult<AuthorizationPolicy?>(policy);
        }

        return _fallbackProvider.GetPolicyAsync(policyName);
    }
}

/// <summary>
/// Handler يتحقق من وجود claim "permission" بالقيمة المطلوبة.
/// (مدموج هنا عشان الـ policy provider والـ handler في نفس السياق.)
/// </summary>
public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        // الأدمن (Admin أو SuperAdmin) يتجاوز كل الفحوص
        if (context.User.IsInRole("Admin") || context.User.IsInRole("SuperAdmin"))
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }

        // Wildcard "*" = كل الصلاحيات (للأدمن بشكل ديناميكي)
        if (context.User.HasClaim(c => c.Type == "permission" && c.Value == "*"))
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }

        if (context.User.HasClaim(c => c.Type == "permission" && c.Value == requirement.PermissionCode))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}

/// <summary>
/// متطلب صلاحية: يحمل كود الصلاحية المطلوب فحصها.
/// </summary>
public class PermissionRequirement : IAuthorizationRequirement
{
    public string PermissionCode { get; }
    public PermissionRequirement(string permissionCode) => PermissionCode = permissionCode;
}

/// <summary>
/// دوال مساعدة لفحص الصلاحية داخل الـ Razor components (AuthorizeView وما شابه).
/// </summary>
public static class PermissionExtensions
{
    /// <summary>هل المستخدم الحالي عنده صلاحية معينة؟ (الأدمن أو SuperAdmin يتجاوز.)</summary>
    public static bool HasPermission(this ClaimsPrincipal user, string permissionCode)
    {
        if (user.IsInRole("Admin") || user.IsInRole("SuperAdmin"))
            return true;

        // Wildcard "*" = كل الصلاحيات
        if (user.HasClaim(c => c.Type == "permission" && c.Value == "*"))
            return true;

        return user.HasClaim(c => c.Type == "permission" && c.Value == permissionCode);
    }

    /// <summary>كل أكواد الصلاحيات للمستخدم الحالي.</summary>
    public static IEnumerable<string> GetPermissions(this ClaimsPrincipal user)
        => user.FindAll("permission").Select(c => c.Value);

    /// <summary>رقم تعريف المستخدم من الـ claims (أو null).</summary>
    public static int? GetUserId(this ClaimsPrincipal user)
    {
        var idClaim = user.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        return int.TryParse(idClaim, out var id) ? id : null;
    }
}
