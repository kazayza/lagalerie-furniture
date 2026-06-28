using LagalerieFurniture.Data;
using LagalerieFurniture.Models;
using Microsoft.EntityFrameworkCore;
using MudBlazor;

namespace LagalerieFurniture.Services;

/// <summary>
/// تنفيذ نظام الصلاحيات: يجمع صلاحيات الدور ثم يطبّق الاستثناءات الشخصية.
/// كل القراءات AsNoTracking لأداء أعلى.
/// </summary>
public class PermissionService : IPermissionService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<PermissionService> _logger;

    public PermissionService(ApplicationDbContext context, ILogger<PermissionService> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<HashSet<string>> GetEffectivePermissionCodesAsync(int userId)
    {
        var now = DateTime.UtcNow;

        // 1) جلب المستخدم (مع دور نشط غير محذوف)
        var user = await _context.Users
            .AsNoTracking()
            .Select(u => new { u.Id, u.RoleId })
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
            return new HashSet<string>();

        // 2) صلاحيات الدور الممنوحة
        var rolePermissionCodes = await _context.RolePermissions
            .AsNoTracking()
            .Where(rp => rp.RoleId == user.RoleId && rp.IsGranted)
            .Select(rp => rp.Permission.Code)
            .ToListAsync();

        var effective = new HashSet<string>(rolePermissionCodes, StringComparer.OrdinalIgnoreCase);

        // 3) الاستثناءات الشخصية الفعّالة (داخل نطاق التاريخ إن وُجد)
        var userOverrides = await _context.UserPermissions
            .AsNoTracking()
            .Where(up => up.UserId == userId
                         && (up.StartDate == null || up.StartDate <= now)
                         && (up.EndDate == null || up.EndDate >= now))
            .Select(up => new { up.IsGranted, PermissionCode = up.Permission.Code })
            .ToListAsync();

        foreach (var ov in userOverrides)
        {
            if (ov.IsGranted)
                effective.Add(ov.PermissionCode);
            else
                effective.Remove(ov.PermissionCode);
        }

        return effective;
    }

    /// <inheritdoc/>
    public async Task<bool> HasPermissionAsync(int userId, string permissionCode)
    {
        var codes = await GetEffectivePermissionCodesAsync(userId);
        return codes.Contains(permissionCode);
    }

    /// <inheritdoc/>
    public async Task<List<NavigationModuleDto>> GetAccessibleModulesAsync(int userId)
    {
        // كل الموديولات المفعّلة مرتبة
        var modules = await _context.Modules
            .AsNoTracking()
            .Where(m => m.IsEnabled)
            .OrderBy(m => m.SortOrder)
            .Select(m => new
            {
                m.Id,
                m.Name,
                m.DisplayName,
                m.Icon,
                m.SortOrder
            })
            .ToListAsync();

        var effective = await GetEffectivePermissionCodesAsync(userId);

        var result = new List<NavigationModuleDto>();
        foreach (var m in modules)
        {
            var links = GetModuleLinks(m.Name);
            // الفلترة: نظهر اللينكات اللي المستخدم عندها صلاحية (أو اللينك مفيشوش شرط صلاحية)
            var accessibleLinks = links
                .Where(l => string.IsNullOrEmpty(l.PermissionCode)
                            || effective.Contains(l.PermissionCode))
                .ToList();

            if (accessibleLinks.Count == 0)
                continue; // الموديول كله مختفي لو مفيش أي لينك متاح

            result.Add(new NavigationModuleDto
            {
                Id = m.Id,
                Name = m.Name,
                DisplayName = m.DisplayName,
                Icon = m.Icon,
                SortOrder = m.SortOrder,
                Links = accessibleLinks
            });
        }

        return result;
    }

    /// <inheritdoc/>
    public async Task<List<Permission>> GetAllPermissionsAsync()
    {
        return await _context.Permissions
            .AsNoTracking()
            .Include(p => p.Module)
            .OrderBy(p => p.Module.SortOrder).ThenBy(p => p.Category).ThenBy(p => p.Id)
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<List<Module>> GetAllModulesAsync()
    {
        return await _context.Modules
            .AsNoTracking()
            .Where(m => m.IsEnabled)
            .OrderBy(m => m.SortOrder)
            .ToListAsync();
    }

    /// <summary>
    /// خريطة ثابتة: اسم الموديول ← لينكاته (Href + Icon + شرط الصلاحية).
    /// ده الـ "navigation catalog" — لما نضيف موديول جديد نضيف له case هنا
    /// وفي نفس الوقت نضيفه في seed الـ modules/permissions.
    /// </summary>
    private static List<NavigationLinkDto> GetModuleLinks(string moduleName) => moduleName switch
    {
        "Dashboard" => new()
        {
            new() { Label = "لوحة التحكم", Href = "/", Icon = Icons.Material.Filled.Dashboard }
        },
        "Users" => new()
        {
            new() { Label = "المستخدمون", Href = "/users", Icon = Icons.Material.Filled.People, PermissionCode = "users.view" },
            new() { Label = "الأدوار", Href = "/roles", Icon = Icons.Material.Filled.ManageAccounts, PermissionCode = "roles.view" },
        },
        "Customers" => new()
        {
            new() { Label = "العملاء", Href = "/customers", Icon = Icons.Material.Filled.Group, PermissionCode = "customers.view" },
        },
        "Invoices" => new()
        {
            new() { Label = "الفواتير", Href = "/invoices", Icon = Icons.Material.Filled.ReceiptLong, PermissionCode = "invoices.view" },
        },
        "Inventory" => new()
        {
            new() { Label = "المخزون", Href = "/inventory", Icon = Icons.Material.Filled.Inventory2, PermissionCode = "inventory.view" },
            new() { Label = "المستودعات", Href = "/warehouses", Icon = Icons.Material.Filled.Warehouse, PermissionCode = "inventory.warehouses" },
            new() { Label = "الموردون", Href = "/suppliers", Icon = Icons.Material.Filled.LocalShipping, PermissionCode = "inventory.suppliers" },
        },
        "Production" => new()
        {
            new() { Label = "أوامر الإنتاج", Href = "/production-orders", Icon = Icons.Material.Filled.PrecisionManufacturing, PermissionCode = "production.view" },
        },
        "Finance" => new()
        {
            new() { Label = "الحسابات", Href = "/accounts", Icon = Icons.Material.Filled.AccountBalance, PermissionCode = "finance.view" },
            new() { Label = "التقارير", Href = "/reports", Icon = Icons.Material.Filled.Assessment, PermissionCode = "finance.reports" },
        },
        "Settings" => new()
        {
            new() { Label = "إعدادات النظام", Href = "/settings", Icon = Icons.Material.Filled.Settings, PermissionCode = "settings.view" },
        },
        _ => new()
    };
}
