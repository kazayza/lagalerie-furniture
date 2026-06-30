using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace LagalerieFurniture.Services;

/// <summary>
/// يوفّر policies بشكل ديناميكي: أي اسم policy يعبّر عن كود صلاحية
/// مثل SET_VIEW أو SALES_VIEW أو users.view يتحول إلى PermissionRequirement.
///
/// مهم: لا نستخدم RequireClaim مباشرة هنا، لأن RequireClaim لا يفهم wildcard (*)
/// ولا Admin/SuperAdmin bypass. لذلك نمرر الفحص إلى PermissionAuthorizationHandler.
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
        // أي policy اسمها كود صلاحية — أمثلة:
        // SET_VIEW, SET_EDIT, DASH_VIEW, SALES_VIEW, users.view
        // نحولها إلى Requirement مخصص علشان يدعم:
        // - Admin / SuperAdmin bypass
        // - permission wildcard = "*"
        // - claim permission المطابق للكود
        if (IsPermissionPolicy(policyName))
        {
            var policy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .AddRequirements(new PermissionRequirement(policyName))
                .Build();

            return Task.FromResult<AuthorizationPolicy?>(policy);
        }

        // أي policy عادية مثل "Admin" تروح للـ fallback provider
        return _fallbackProvider.GetPolicyAsync(policyName);
    }

    private static bool IsPermissionPolicy(string policyName)
    {
        if (string.IsNullOrWhiteSpace(policyName))
            return false;

        // سياسات الصلاحيات عندنا إما UPPER_SNAKE مثل SET_VIEW
        // أو dot notation مثل users.view
        return policyName.Contains('_') || policyName.Contains('.');
    }
}

/// <summary>
/// Handler يتحقق من صلاحية المستخدم.
/// يدعم:
/// - Admin / SuperAdmin bypass
/// - wildcard permission = "*"
/// - permission claim مطابق للكود المطلوب
/// </summary>
public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        if (context.User?.Identity?.IsAuthenticated != true)
            return Task.CompletedTask;

        // الأدمن يتجاوز كل الفحوص
        if (context.User.IsInRole("Admin") || context.User.IsInRole("SuperAdmin"))
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }

        // Wildcard "*" = كل الصلاحيات
        if (context.User.HasClaim(c => c.Type == "permission" && c.Value == "*"))
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }

        // الصلاحية المطلوبة موجودة كـ claim
        if (context.User.HasClaim(c =>
                c.Type == "permission" &&
                string.Equals(c.Value, requirement.PermissionCode, StringComparison.OrdinalIgnoreCase)))
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
/// دوال مساعدة لفحص الصلاحية داخل Razor components.
/// </summary>
public static class PermissionExtensions
{
    /// <summary>هل المستخدم الحالي عنده صلاحية معينة؟</summary>
    public static bool HasPermission(this ClaimsPrincipal user, string permissionCode)
    {
        if (user?.Identity?.IsAuthenticated != true)
            return false;

        if (user.IsInRole("Admin") || user.IsInRole("SuperAdmin"))
            return true;

        if (user.HasClaim(c => c.Type == "permission" && c.Value == "*"))
            return true;

        return user.HasClaim(c =>
            c.Type == "permission" &&
            string.Equals(c.Value, permissionCode, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>كل أكواد الصلاحيات للمستخدم الحالي.</summary>
    public static IEnumerable<string> GetPermissions(this ClaimsPrincipal user)
        => user.FindAll("permission").Select(c => c.Value);

    /// <summary>رقم تعريف المستخدم من الـ claims أو null.</summary>
    public static int? GetUserId(this ClaimsPrincipal user)
    {
        var idClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return int.TryParse(idClaim, out var id) ? id : null;
    }
}
