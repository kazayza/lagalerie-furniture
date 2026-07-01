using LagalerieFurniture.Data;
using LagalerieFurniture.Models;
using Microsoft.EntityFrameworkCore;
using MudBlazor;

namespace LagalerieFurniture.Services;

/// <summary>
/// تنفيذ نظام الصلاحيات: يجمع صلاحيات الدور ثم يطبّق الاستثناءات الشخصية.
/// كل القراءات AsNoTracking لأداء أعلى.
/// يستخدم IDbContextFactory عشان كل عملية DB تـ create DbContext مستقل —
/// ضروري في Blazor Server عشان نتجنب concurrent access.
/// </summary>
public class PermissionService : IPermissionService
{
    private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
    private readonly ILogger<PermissionService> _logger;

    public PermissionService(IDbContextFactory<ApplicationDbContext> contextFactory, ILogger<PermissionService> logger)
    {
        _contextFactory = contextFactory;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<HashSet<string>> GetEffectivePermissionCodesAsync(int userId)
    {
        var now = DateTime.UtcNow;
        using var context = _contextFactory.CreateDbContext();

        // 1) جلب المستخدم مع اسم الدور (للكشف عن Admin)
        var user = await context.Users
            .AsNoTracking()
            .Select(u => new { u.Id, u.RoleId, RoleName = u.Role != null ? u.Role.Name : null })
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
            return new HashSet<string>();

        // Admin bypass: SuperAdmin و Admin لهم كل الصلاحيات دائماً
        var isAdmin = string.Equals(user.RoleName, "Admin", StringComparison.OrdinalIgnoreCase)
                   || string.Equals(user.RoleName, "SuperAdmin", StringComparison.OrdinalIgnoreCase);

        // 2) صلاحيات الدور الممنوحة
        var rolePermissionCodes = await context.RolePermissions
            .AsNoTracking()
            .Where(rp => rp.RoleId == user.RoleId && rp.IsGranted)
            .Select(rp => rp.Permission.Code)
            .ToListAsync();

        var effective = new HashSet<string>(rolePermissionCodes, StringComparer.OrdinalIgnoreCase);

        if (isAdmin)
        {
            // نضيف "wildcard" مميز — GetAccessibleModulesAsync بيتعامل معاه
            effective.Add("*");
        }

        // 3) الاستثناءات الشخصية الفعّالة (داخل نطاق التاريخ إن وُجد)
        var userOverrides = await context.UserPermissions
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
        var effective = await GetEffectivePermissionCodesAsync(userId);
        var isAdmin = effective.Contains("*");

        _logger.LogInformation(
            "GetAccessibleModulesAsync: userId={UserId}, isAdmin={IsAdmin}, permissionCount={Count}, codes={Codes}",
            userId, isAdmin, effective.Count, string.Join(",", effective.Take(20)));

        using var context = _contextFactory.CreateDbContext();

        // كل الموديولات المفعّلة مرتبة
        var modules = await context.Modules
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

        // Fallback 1: لو جدول Modules فاضي، نرجّع موديولات افتراضية كاملة
        if (modules.Count == 0)
        {
            _logger.LogWarning("جدول Modules فاضي — رجّع fallback modules ثابتة");
            return GetFallbackModules(isAdmin ? null : effective);
        }

        var result = new List<NavigationModuleDto>();
        foreach (var m in modules)
        {
            var links = GetModuleLinks(m.Name);

            // لو الـ switch ما رجّعش أي لينكات (اسم الموديول مش متطابق)،
            // نحاول case-insensitive match مع الموديولات الافتراضية
            if (links.Count == 0)
            {
                links = GetModuleLinksCaseInsensitive(m.Name);
            }

            // الفلترة: نظهر اللينكات اللي المستخدم عندها صلاحية
            // (أو اللينك مفيهوش شرط صلاحية، أو المستخدم Admin)
            var accessibleLinks = links
                .Where(l => string.IsNullOrEmpty(l.PermissionCode)
                            || isAdmin
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

        // Fallback 2: لو موديولات موجودة بس ولا لينك اتطابق (DB مخترب)، رجّع fallback
        if (result.Count == 0)
        {
            _logger.LogWarning("موديولات موجودة في DB بس ولا لينك اتطابق — رجّع fallback");
            return GetFallbackModules(isAdmin ? null : effective);
        }

        // حقن موديول Users يدوياً (مش موجود في جدول Modules في الـ DB)
        // الـ admin وشواللي ليهم SET_VIEW يقدروا يشوفوه
        var hasSetView = isAdmin || effective.Contains("SET_VIEW");
        if (hasSetView)
        {
            // نتأكد إنه مش متكرر (لو DB فيه Users بالفعل)
            if (!result.Any(r => string.Equals(r.Name, "Users", StringComparison.OrdinalIgnoreCase)))
            {
                var usersLinks = GetModuleLinks("Users")
                    .Where(l => string.IsNullOrEmpty(l.PermissionCode) || isAdmin || effective.Contains(l.PermissionCode))
                    .ToList();

                if (usersLinks.Count > 0)
                {
                    result.Insert(1, new NavigationModuleDto
                    {
                        Id = 0,
                        Name = "Users",
                        DisplayName = "المستخدمون والأدوار",
                        Icon = Icons.Material.Filled.People,
                        SortOrder = 2,
                        Links = usersLinks
                    });
                    _logger.LogInformation("تم حقن موديول Users يدوياً");
                }
            }
        }

        _logger.LogInformation("GetAccessibleModulesAsync: رجّع {Count} موديول", result.Count);
        return result;
    }

    /// <inheritdoc/>
    public async Task<List<Permission>> GetAllPermissionsAsync()
    {
        using var context = _contextFactory.CreateDbContext();
        return await context.Permissions
            .AsNoTracking()
            .Include(p => p.Module)
            .OrderBy(p => p.Module.SortOrder).ThenBy(p => p.Category).ThenBy(p => p.Id)
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<List<Module>> GetAllModulesAsync()
    {
        using var context = _contextFactory.CreateDbContext();
        return await context.Modules
            .AsNoTracking()
            .Where(m => m.IsEnabled)
            .OrderBy(m => m.SortOrder)
            .ToListAsync();
    }

    /// <summary>
    /// خريطة ثابتة: اسم الموديول (بـ PascalCase زي ما هو في DB) ← لينكاته.
    /// الأسماء مطابقة لـ Modules table في الـ DB:
    ///   Dashboard, Products, CRM, Sales, Purchasing, Inventory, Production,
    ///   Finance, HR, Projects, Delivery, Complaints, Reports, Settings, Assets
    ///
    /// أكواد الصلاحيات (Permission Codes) في الـ DB بصيغة UPPER_SNAKE:
    ///   DASH_VIEW, PROD_VIEW, CRM_VIEW, SALES_VIEW, PUR_VIEW, INV_VIEW,
    ///   FIN_VIEW, HR_VIEW, REP_VIEW, SET_VIEW, ASS_VIEW, COMP_VIEW, ...
    ///
    /// ملاحظة: موديول Users مش موجود في DB — نضيفه يدوياً ونستخدم SET_VIEW
    /// كصلاحية بديلة (المستخدمين إداريون لهم صلاحيات الإعدادات).
    /// </summary>
    private static List<NavigationLinkDto> GetModuleLinks(string moduleName) => moduleName switch
    {
        "Dashboard" => new()
        {
            new() { Label = "لوحة التحكم", Href = "/", Icon = Icons.Material.Filled.Dashboard, PermissionCode = "DASH_VIEW" }
        },
        "Security" => new()
{
    new() { Label = "المستخدمون", Href = "/users", Icon = Icons.Material.Filled.People, PermissionCode = "USR_VIEW" },
    new() { Label = "الأدوار", Href = "/roles", Icon = Icons.Material.Filled.ManageAccounts, PermissionCode = "ROLE_VIEW" },
},
        // Users module مش موجود في DB — نضيفه يدوياً مع ربطه بصلاحية SET_VIEW
        "Users" => new()
        {
            new() { Label = "المستخدمون", Href = "/users", Icon = Icons.Material.Filled.People, PermissionCode = "SET_VIEW" },
            new() { Label = "الأدوار", Href = "/roles", Icon = Icons.Material.Filled.ManageAccounts, PermissionCode = "SET_VIEW" },
        },
        "Products" => new()
        {
            new() { Label = "المنتجات", Href = "/products", Icon = Icons.Material.Filled.Inventory2, PermissionCode = "PROD_VIEW" },
        },
        "CRM" => new()
        {
            new() { Label = "العملاء", Href = "/customers", Icon = Icons.Material.Filled.Group, PermissionCode = "CRM_VIEW" },
        },
        "Sales" => new()
        {
            new() { Label = "الفواتير", Href = "/invoices", Icon = Icons.Material.Filled.ReceiptLong, PermissionCode = "SALES_VIEW" },
            new() { Label = "المدفوعات", Href = "/payments", Icon = Icons.Material.Filled.Payment, PermissionCode = "SALES_VIEW" },
        },
        "Purchasing" => new()
        {
            new() { Label = "أوامر الشراء", Href = "/purchase-orders", Icon = Icons.Material.Filled.ShoppingCart, PermissionCode = "PUR_VIEW" },
            new() { Label = "الموردون", Href = "/suppliers", Icon = Icons.Material.Filled.LocalShipping, PermissionCode = "PUR_VIEW" },
        },
        "Inventory" => new()
        {
            new() { Label = "المخزون", Href = "/inventory", Icon = Icons.Material.Filled.Inventory2, PermissionCode = "INV_VIEW" },
            new() { Label = "المستودعات", Href = "/warehouses", Icon = Icons.Material.Filled.Warehouse, PermissionCode = "INV_VIEW" },
            new() { Label = "أذون الاستلام", Href = "/goods-receipts", Icon = Icons.Material.Filled.MoveToInbox, PermissionCode = "INV_VIEW" },
        },
        "Production" => new()
        {
            new() { Label = "أوامر الإنتاج", Href = "/production-orders", Icon = Icons.Material.Filled.PrecisionManufacturing, PermissionCode = "INV_VIEW" },
            new() { Label = "شجرة المواد (BOM)", Href = "/bom", Icon = Icons.Material.Filled.AccountTree, PermissionCode = "INV_VIEW" },
        },
        "Finance" => new()
        {
            new() { Label = "القيود اليومية", Href = "/journal-entries", Icon = Icons.Material.Filled.Receipt, PermissionCode = "FIN_VIEW" },
            new() { Label = "دليل الحسابات", Href = "/chart-of-accounts", Icon = Icons.Material.Filled.AccountBalance, PermissionCode = "FIN_VIEW" },
            new() { Label = "الصناديق", Href = "/cash-registers", Icon = Icons.Material.Filled.AccountBalanceWallet, PermissionCode = "FIN_VIEW" },
        },
        "HR" => new()
        {
            new() { Label = "الموظفون", Href = "/employees", Icon = Icons.Material.Filled.Badge, PermissionCode = "HR_VIEW" },
            new() { Label = "الحضور", Href = "/attendance", Icon = Icons.Material.Filled.Schedule, PermissionCode = "HR_VIEW" },
            new() { Label = "الرواتب", Href = "/payrolls", Icon = Icons.Material.Filled.Payments, PermissionCode = "HR_VIEW" },
        },
        "Complaints" => new()
        {
            new() { Label = "الشكاوى", Href = "/complaints", Icon = Icons.Material.Filled.Feedback, PermissionCode = "COMP_VIEW" },
        },
        "Reports" => new()
        {
            new() { Label = "التقارير", Href = "/reports", Icon = Icons.Material.Filled.Assessment, PermissionCode = "REP_VIEW" },
        },
        "Settings" => new()
        {
            new() { Label = "إعدادات النظام", Href = "/settings", Icon = Icons.Material.Filled.Settings, PermissionCode = "SET_VIEW" },
            new() { Label = "الفروع والأقسام", Href = "/branches", Icon = Icons.Material.Filled.Business, PermissionCode = "SET_VIEW" },
        },
        "Assets" => new()
        {
            new() { Label = "الأصول الثابتة", Href = "/assets", Icon = Icons.Material.Filled.BusinessCenter, PermissionCode = "ASS_VIEW" },
        },
        // موديولات غير مفعّلة في الـ DB حالياً (Projects, Delivery) — نرجّع فاضي
        _ => new()
    };

    /// <summary>
    /// محاولة case-insensitive match لاسم الموديول — عشان نتأكد إن
    /// لو DB فيه "dashboard" بدل "Dashboard" أو "inventory" بدل "Inventory"
    /// الـ links تشتغل برضه.
    /// </summary>
    private static List<NavigationLinkDto> GetModuleLinksCaseInsensitive(string moduleName)
    {
        if (string.IsNullOrWhiteSpace(moduleName))
            return new List<NavigationLinkDto>();

        var known = new Dictionary<string, List<NavigationLinkDto>>(StringComparer.OrdinalIgnoreCase)
        {
            ["Dashboard"] = GetModuleLinks("Dashboard"),
            ["Users"] = GetModuleLinks("Users"),
            ["Products"] = GetModuleLinks("Products"),
            ["CRM"] = GetModuleLinks("CRM"),
            ["Sales"] = GetModuleLinks("Sales"),
            ["Purchasing"] = GetModuleLinks("Purchasing"),
            ["Inventory"] = GetModuleLinks("Inventory"),
            ["Production"] = GetModuleLinks("Production"),
            ["Finance"] = GetModuleLinks("Finance"),
            ["HR"] = GetModuleLinks("HR"),
            ["Complaints"] = GetModuleLinks("Complaints"),
            ["Reports"] = GetModuleLinks("Reports"),
            ["Settings"] = GetModuleLinks("Settings"),
            ["Assets"] = GetModuleLinks("Assets"),
        };

        return known.TryGetValue(moduleName.Trim(), out var v) ? v : new List<NavigationLinkDto>();
    }

    /// <summary>
    /// يرجّع موديولات افتراضية كاملة (hardcoded) لما جدول Modules فاضي أو مخترب.
    /// إذا كان effective=null (Admin) يرجّع كل اللينكات، وإلا يفلتر حسب الصلاحيات.
    /// الأسماء مطابقة لـ DB Modules table.
    /// </summary>
    private static List<NavigationModuleDto> GetFallbackModules(HashSet<string>? effective)
    {
        var allModules = new List<(string Name, string DisplayName, int SortOrder)>
        {
            ("Dashboard", "لوحة التحكم", 1),
            ("Users", "المستخدمون والأدوار", 2),
            ("Products", "المنتجات", 3),
            ("CRM", "العملاء", 4),
            ("Sales", "المبيعات", 5),
            ("Purchasing", "المشتريات", 6),
            ("Inventory", "المخازن", 7),
            ("Production", "الإنتاج", 8),
            ("Finance", "المالية", 9),
            ("HR", "الموارد البشرية", 10),
            ("Complaints", "الشكاوى", 12),
            ("Reports", "التقارير", 13),
            ("Settings", "الإعدادات", 14),
            ("Assets", "الأصول الثابتة", 15),
        };

        var result = new List<NavigationModuleDto>();
        foreach (var (name, displayName, sortOrder) in allModules)
        {
            var links = GetModuleLinks(name);

            // فلترة (skip لو Admin لأن effective = null)
            var accessibleLinks = effective == null
                ? links
                : links.Where(l => string.IsNullOrEmpty(l.PermissionCode) || effective.Contains(l.PermissionCode)).ToList();

            if (accessibleLinks.Count == 0) continue;

            result.Add(new NavigationModuleDto
            {
                Id = 0,
                Name = name,
                DisplayName = displayName,
                Icon = MapModuleNameToIcon(name),
                SortOrder = sortOrder,
                Links = accessibleLinks
            });
        }

        return result;
    }

    /// <summary>Maps module name to a Material icon for fallback case.</summary>
    private static string MapModuleNameToIcon(string name) => name switch
    {
        "Dashboard" => Icons.Material.Filled.Dashboard,
        "Users" => Icons.Material.Filled.People,
        "Products" => Icons.Material.Filled.Inventory2,
        "CRM" => Icons.Material.Filled.Group,
        "Sales" => Icons.Material.Filled.ReceiptLong,
        "Purchasing" => Icons.Material.Filled.ShoppingCart,
        "Inventory" => Icons.Material.Filled.Warehouse,
        "Production" => Icons.Material.Filled.PrecisionManufacturing,
        "Finance" => Icons.Material.Filled.AccountBalance,
        "HR" => Icons.Material.Filled.Badge,
        "Complaints" => Icons.Material.Filled.Feedback,
        "Reports" => Icons.Material.Filled.Assessment,
        "Settings" => Icons.Material.Filled.Settings,
        "Assets" => Icons.Material.Filled.BusinessCenter,
        _ => Icons.Material.Filled.Circle
    };
}