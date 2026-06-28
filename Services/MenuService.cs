using LagalerieFurniture.Data;
using LagalerieFurniture.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using MudBlazor;

namespace LagalerieFurniture.Services;

public class MenuService : IMenuService
{
    private readonly ApplicationDbContext _context;
    private readonly IPermissionService _permissionService;
    private readonly IJSRuntime _js;
    private readonly ILogger<MenuService> _logger;
    private const string StorageKey = "lagalerie_sidebar_mini";

    public MenuService(
        ApplicationDbContext context,
        IPermissionService permissionService,
        IJSRuntime js,
        ILogger<MenuService> logger)
    {
        _context = context;
        _permissionService = permissionService;
        _js = js;
        _logger = logger;
    }

    public async Task<List<NavMenuGroup>> GetUserMenuAsync(int userId)
    {
        var accessibleModules = await _permissionService.GetAccessibleModulesAsync(userId);

        if (accessibleModules.Count == 0)
        {
            return new List<NavMenuGroup>
            {
                new()
                {
                    Id = 0, Name = "Dashboard", DisplayName = "لوحة التحكم",
                    Icon = Icons.Material.Filled.Dashboard, SortOrder = 0, IsExpanded = true,
                    Items = new List<NavMenuItem>
                    {
                        new() { Label = "الرئيسية", Href = "/", Icon = Icons.Material.Filled.Dashboard }
                    }
                }
            };
        }

        var menuGroups = accessibleModules
            .OrderBy(m => m.SortOrder)
            .Select(m => new NavMenuGroup
            {
                Id = m.Id,
                Name = m.Name,
                DisplayName = m.DisplayName,
                Icon = MapModuleIcon(m.Name),
                SortOrder = m.SortOrder,
                IsExpanded = true,
                Items = m.Links.Select(l => new NavMenuItem
                {
                    Label = l.Label,
                    Href = l.Href,
                    Icon = !string.IsNullOrEmpty(l.Icon) ? l.Icon : MapDefaultIconForModule(m.Name),
                    PermissionCode = l.PermissionCode,
                    Badge = null
                }).ToList()
            })
            .ToList();

        return menuGroups;
    }

    public async Task<bool> GetSidebarMiniModeAsync()
    {
        try
        {
            var value = await _js.InvokeAsync<string>("localStorage.getItem", StorageKey);
            return value == "true";
        }
        catch { return false; }
    }

    public async Task SetSidebarMiniModeAsync(bool isMini)
    {
        try
        {
            await _js.InvokeVoidAsync("localStorage.setItem", StorageKey, isMini ? "true" : "false");
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "فشل حفظ حالة القائمة في localStorage");
        }
    }

    public async Task<int> GetUnreadNotificationCountAsync(int userId)
    {
        try
        {
            return await _context.Notifications
                .AsNoTracking()
                .CountAsync(n => n.UserId == userId && !n.IsRead);
        }
        catch { return 0; }
    }

    private static string MapModuleIcon(string moduleName) => moduleName switch
    {
        "Dashboard" => Icons.Material.Filled.Dashboard,
        "Users" => Icons.Material.Filled.People,
        "Customers" => Icons.Material.Filled.Group,
        "Invoices" => Icons.Material.Filled.ReceiptLong,
        "Sales" => Icons.Material.Filled.ShoppingCart,
        "Inventory" => Icons.Material.Filled.Inventory2,
        "Warehouses" => Icons.Material.Filled.Warehouse,
        "Suppliers" => Icons.Material.Filled.LocalShipping,
        "Production" => Icons.Material.Filled.PrecisionManufacturing,
        "Manufacturing" => Icons.Material.Filled.Factory,
        "Finance" => Icons.Material.Filled.AccountBalance,
        "Accounting" => Icons.Material.Filled.AccountBalance,
        "HR" => Icons.Material.Filled.Badge,
        "Employees" => Icons.Material.Filled.Badge,
        "Payroll" => Icons.Material.Filled.Payments,
        "Attendance" => Icons.Material.Filled.Schedule,
        "Assets" => Icons.Material.Filled.BusinessCenter,
        "Reports" => Icons.Material.Filled.Assessment,
        "Settings" => Icons.Material.Filled.Settings,
        "System" => Icons.Material.Filled.AdminPanelSettings,
        "Notifications" => Icons.Material.Filled.Notifications,
        "Calendar" => Icons.Material.Filled.CalendarMonth,
        "Tasks" => Icons.Material.Filled.Task,
        "CRM" => Icons.Material.Filled.SupportAgent,
        "Complaints" => Icons.Material.Filled.Feedback,
        "Quality" => Icons.Material.Filled.Quality,
        "Purchases" => Icons.Material.Filled.ShoppingBag,
        "Quotations" => Icons.Material.Filled.RequestQuote,
        _ => Icons.Material.Filled.Circle
    };

    private static string MapDefaultIconForModule(string moduleName) => moduleName switch
    {
        "Dashboard" => Icons.Material.Filled.Dashboard,
        "Invoices" => Icons.Material.Filled.Receipt,
        "Inventory" => Icons.Material.Filled.Inventory,
        "Production" => Icons.Material.Filled.Handyman,
        "Finance" => Icons.Material.Filled.AccountBalanceWallet,
        "HR" => Icons.Material.Filled.Person,
        "Reports" => Icons.Material.Filled.BarChart,
        "Settings" => Icons.Material.Filled.Tune,
        _ => Icons.Material.Filled.ChevronLeft
    };
}

