using LagalerieFurniture.Data;
using LagalerieFurniture.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace LagalerieFurniture.Services;

/// <summary>
/// تنفيذ <see cref="IDashboardService"/> — يجيب بيانات حقيقية من الـ DB
/// ويحترم صلاحيات المستخدم (مفيش بيانات تظهر لو المستخدم معندوش صلاحية شوفتها).
/// كل الاستعلامات AsNoTracking لأداء أعلى.
/// يستخدم IDbContextFactory عشان كل عملية DB تـ create DbContext مستقل.
/// </summary>
public class DashboardService : IDashboardService
{
    private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
    private readonly ILogger<DashboardService> _logger;

    public DashboardService(IDbContextFactory<ApplicationDbContext> contextFactory, ILogger<DashboardService> logger)
    {
        _contextFactory = contextFactory;
        _logger = logger;
    }

    public async Task<DashboardKpisDto> GetKpisAsync(int userId, HashSet<string> permissions)
    {
        var dto = new DashboardKpisDto();
        var today = DateTime.UtcNow.Date;
        var monthStart = new DateTime(today.Year, today.Month, 1);
        var prevMonthStart = monthStart.AddMonths(-1);

        try
        {
            using var context = _contextFactory.CreateDbContext();

            // المبيعات اليوم + عدد الفواتير
            if (permissions.Contains("SALES_VIEW"))
            {
                var todayInvoices = await context.Invoices
                    .AsNoTracking()
                    .Where(i => !i.IsDeleted && i.InvoiceDate.Date == today)
                    .GroupBy(_ => 1)
                    .Select(g => new
                    {
                        Total = g.Sum(i => i.TotalAmount),
                        Count = g.Count()
                    })
                    .FirstOrDefaultAsync();
                dto.TodaySales = todayInvoices?.Total ?? 0;
                dto.TodayInvoicesCount = todayInvoices?.Count ?? 0;

                // مبيعات الشهر + الدلتا مقارنة بالشهر السابق
                var monthSales = await context.Invoices
                    .AsNoTracking()
                    .Where(i => !i.IsDeleted && i.InvoiceDate >= monthStart)
                    .SumAsync(i => i.TotalAmount);

                var prevMonthSales = await context.Invoices
                    .AsNoTracking()
                    .Where(i => !i.IsDeleted && i.InvoiceDate >= prevMonthStart && i.InvoiceDate < monthStart)
                    .SumAsync(i => i.TotalAmount);

                dto.MonthSales = monthSales;
                dto.MonthSalesDeltaPct = prevMonthSales > 0
                    ? Math.Round((monthSales - prevMonthSales) / prevMonthSales * 100, 1)
                    : (monthSales > 0 ? 100m : 0);

                // المستحقات (remaining amount على الفواتير غير المدفوعة بالكامل)
                dto.OutstandingReceivables = await context.Invoices
                    .AsNoTracking()
                    .Where(i => !i.IsDeleted && i.RemainingAmount > 0)
                    .SumAsync(i => i.RemainingAmount);

                dto.PendingInvoices = await context.Invoices
                    .AsNoTracking()
                    .CountAsync(i => !i.IsDeleted && i.Status != "Paid" && i.Status != "Cancelled");
            }

            // العملاء النشطون
            if (permissions.Contains("CRM_VIEW"))
            {
                dto.ActiveCustomersCount = await context.Customers
                    .AsNoTracking()
                    .CountAsync(c => !c.IsDeleted && c.IsActive);
            }

            // المنتجات النشطة
            if (permissions.Contains("INV_VIEW"))
            {
                dto.ActiveProductsCount = await context.Products
                    .AsNoTracking()
                    .CountAsync(p => !p.IsDeleted && p.IsActive);

                // عدد الأصناف تحت الحد الأدنى
                dto.LowStockCount = await context.Inventories
                    .AsNoTracking()
                    .CountAsync(i => i.QuantityOnHand <= i.ReorderLevel);
            }

            // أوامر الإنتاج المعلّقة
            if (permissions.Contains("INV_VIEW"))
            {
                dto.PendingProductionOrders = await context.ProductionOrders
                    .AsNoTracking()
                    .CountAsync(p => p.Status != "Completed" && p.Status != "Cancelled");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "فشل تحميل KPIs للداشبورد (userId={UserId})", userId);
        }

        return dto;
    }

    public async Task<List<SalesPointDto>> GetLast7DaysSalesAsync(int userId, HashSet<string> permissions)
    {
        if (!permissions.Contains("SALES_VIEW"))
            return new List<SalesPointDto>();

        try
        {
            using var context = _contextFactory.CreateDbContext();
            var startDate = DateTime.UtcNow.Date.AddDays(-6);

            var raw = await context.Invoices
                .AsNoTracking()
                .Where(i => !i.IsDeleted && i.InvoiceDate >= startDate)
                .GroupBy(i => i.InvoiceDate.Date)
                .Select(g => new
                {
                    Date = g.Key,
                    Total = g.Sum(i => i.TotalAmount),
                    Count = g.Count()
                })
                .ToListAsync();

            // نضمن إن كل يوم موجود حتى لو مفيش فواتير
            var result = new List<SalesPointDto>();
            for (var i = 0; i < 7; i++)
            {
                var date = startDate.AddDays(i);
                var point = raw.FirstOrDefault(r => r.Date == date);
                result.Add(new SalesPointDto
                {
                    Date = date,
                    DateLabel = date.ToString("ddd", new System.Globalization.CultureInfo("ar-EG")),
                    Total = point?.Total ?? 0,
                    InvoicesCount = point?.Count ?? 0
                });
            }
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "فشل تحميل مبيعات آخر 7 أيام (userId={UserId})", userId);
            return new List<SalesPointDto>();
        }
    }

    public async Task<List<InvoiceStatusBreakdownDto>> GetInvoiceStatusBreakdownAsync(int userId, HashSet<string> permissions)
    {
        if (!permissions.Contains("SALES_VIEW"))
            return new List<InvoiceStatusBreakdownDto>();

        try
        {
            using var context = _contextFactory.CreateDbContext();
            var raw = await context.Invoices
                .AsNoTracking()
                .Where(i => !i.IsDeleted)
                .GroupBy(i => i.Status)
                .Select(g => new
                {
                    Status = g.Key,
                    Count = g.Count(),
                    Total = g.Sum(i => i.TotalAmount)
                })
                .ToListAsync();

            return raw.Select(r => new InvoiceStatusBreakdownDto
            {
                Status = r.Status,
                StatusLabel = MapInvoiceStatus(r.Status),
                Count = r.Count,
                TotalAmount = r.Total
            }).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "فشل تحميل توزيع حالات الفواتير");
            return new List<InvoiceStatusBreakdownDto>();
        }
    }

    public async Task<List<RecentInvoiceDto>> GetRecentInvoicesAsync(int userId, HashSet<string> permissions, int count = 5)
    {
        if (!permissions.Contains("SALES_VIEW"))
            return new List<RecentInvoiceDto>();

        try
        {
            using var context = _contextFactory.CreateDbContext();
            return await context.Invoices
                .AsNoTracking()
                .Where(i => !i.IsDeleted)
                .OrderByDescending(i => i.CreatedAt)
                .Take(count)
                .Select(i => new RecentInvoiceDto
                {
                    Id = i.Id,
                    InvoiceNumber = i.InvoiceNumber,
                    CustomerName = i.Customer.DisplayName,
                    InvoiceDate = i.InvoiceDate,
                    TotalAmount = i.TotalAmount,
                    PaidAmount = i.PaidAmount,
                    Status = i.Status,
                    Currency = i.Currency
                })
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "فشل تحميل أحدث الفواتير");
            return new List<RecentInvoiceDto>();
        }
    }

    public async Task<List<RecentProductionOrderDto>> GetRecentProductionOrdersAsync(int userId, HashSet<string> permissions, int count = 5)
    {
        if (!permissions.Contains("INV_VIEW"))
            return new List<RecentProductionOrderDto>();

        try
        {
            using var context = _contextFactory.CreateDbContext();
            return await context.ProductionOrders
                .AsNoTracking()
                .OrderByDescending(p => p.CreatedAt)
                .Take(count)
                .Select(p => new RecentProductionOrderDto
                {
                    Id = p.Id,
                    ProductionNumber = p.ProductionNumber,
                    ProductName = p.Product.Name,
                    Quantity = p.Quantity,
                    QuantityCompleted = p.QuantityCompleted,
                    Status = p.Status,
                    Priority = p.Priority,
                    CreatedAt = p.CreatedAt
                })
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "فشل تحميل أحدث أوامر الإنتاج");
            return new List<RecentProductionOrderDto>();
        }
    }

    public async Task<List<LowStockItemDto>> GetLowStockItemsAsync(int userId, HashSet<string> permissions, int count = 5)
    {
        if (!permissions.Contains("INV_VIEW"))
            return new List<LowStockItemDto>();

        try
        {
            using var context = _contextFactory.CreateDbContext();
            return await context.Inventories
                .AsNoTracking()
                .Where(i => i.QuantityOnHand <= i.ReorderLevel)
                .OrderBy(i => i.QuantityOnHand)
                .Take(count)
                .Select(i => new LowStockItemDto
                {
                    ProductId = i.ProductVariant.ProductId,
                    ProductName = i.ProductVariant.Product.Name,
                    Sku = i.ProductVariant.Product.Sku,
                    QuantityOnHand = i.QuantityOnHand,
                    ReorderLevel = i.ReorderLevel,
                    WarehouseName = i.Warehouse.Name
                })
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "فشل تحميل أصناف الحد الأدنى");
            return new List<LowStockItemDto>();
        }
    }

    public async Task<List<TopCustomerDto>> GetTopCustomersAsync(int userId, HashSet<string> permissions, int count = 5)
    {
        if (!permissions.Contains("CRM_VIEW"))
            return new List<TopCustomerDto>();

        try
        {
            using var context = _contextFactory.CreateDbContext();
            return await context.Customers
                .AsNoTracking()
                .Where(c => !c.IsDeleted && c.TotalPurchases > 0)
                .OrderByDescending(c => c.TotalPurchases)
                .Take(count)
                .Select(c => new TopCustomerDto
                {
                    Id = c.Id,
                    DisplayName = c.DisplayName,
                    Phone = c.Phone,
                    TotalOrders = c.TotalOrders,
                    TotalPurchases = c.TotalPurchases,
                    LastPurchaseDate = c.LastPurchaseDate
                })
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "فشل تحميل أعلى العملاء");
            return new List<TopCustomerDto>();
        }
    }

    public async Task<List<ActivityItemDto>> GetRecentActivityAsync(int userId, HashSet<string> permissions, int count = 8)
    {
        // الأنشطة مرتبطة بمستخدم عنده users.view — لو مفيش الصلاحية، نرجع فاضي
        if (!permissions.Contains("SET_VIEW"))
            return new List<ActivityItemDto>();

        try
        {
            using var context = _contextFactory.CreateDbContext();
            var logs = await context.ActivityLogs
                .AsNoTracking()
                .OrderByDescending(a => a.CreatedAt)
                .Take(count)
                .Select(a => new
                {
                    a.Id,
                    ActivityType = a.ActivityType,
                    a.EntityType,
                    a.Description,
                    a.CreatedAt,
                    ActorName = a.User != null ? a.User.DisplayName : "النظام"
                })
                .ToListAsync();

            return logs.Select(a => new ActivityItemDto
            {
                Id = (int)a.Id,
                Action = a.ActivityType ?? "",
                ActorName = a.ActorName ?? "النظام",
                CreatedAt = a.CreatedAt,
                Description = a.Description ?? a.ActivityType ?? "",
                Icon = MapActivityIcon(a.ActivityType),
                Color = MapActivityColor(a.ActivityType)
            }).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "فشل تحميل الأنشطة الأخيرة");
            return new List<ActivityItemDto>();
        }
    }

    // ===== Helpers =====

    private static string MapInvoiceStatus(string status) => status switch
    {
        "Paid" => "مدفوعة",
        "Partial" => "مدفوعة جزئياً",
        "Unpaid" => "غير مدفوعة",
        "Overdue" => "متأخرة",
        "Draft" => "مسودة",
        "Cancelled" => "ملغاة",
        _ => status
    };

    private static string MapActivityIcon(string? action) => action?.ToLowerInvariant() switch
    {
        "create" or "user.create" or "role.create" => "person_add",
        "update" or "user.update" or "role.update" => "edit",
        "delete" or "user.delete" or "role.delete" => "delete",
        "login" => "login",
        "logout" => "logout",
        "payment" => "payment",
        "invoice" => "receipt",
        _ => "info"
    };

    private static string MapActivityColor(string? action) => action?.ToLowerInvariant() switch
    {
        "create" or "user.create" or "role.create" => "success",
        "update" or "user.update" or "role.update" => "primary",
        "delete" or "user.delete" or "role.delete" => "error",
        "login" => "info",
        "logout" => "warning",
        "payment" => "success",
        _ => "default"
    };
}