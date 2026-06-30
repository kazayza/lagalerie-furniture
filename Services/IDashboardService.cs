using LagalerieFurniture.Models;

namespace LagalerieFurniture.Services;

/// <summary>
/// يجيب بيانات الـ Dashboard الحقيقية من الـ DB حسب صلاحيات المستخدم.
/// كل widget بيرجع null لو المستخدم معندوش صلاحية شوفته —
/// فالـ UI بيدّي الكروت اللي عنده صلاحية ليها بس.
/// </summary>
public interface IDashboardService
{
    /// <summary>بيانات الـ KPIs العلوية (مبيعات/فواتير/عملاء/منتجات/إنتاج).</summary>
    Task<DashboardKpisDto> GetKpisAsync(int userId, HashSet<string> permissions);

    /// <summary>سلسلة مبيعات آخر 7 أيام للرسم البياني.</summary>
    Task<List<SalesPointDto>> GetLast7DaysSalesAsync(int userId, HashSet<string> permissions);

    /// <summary>توزيع الفواتير حسب الحالة (paid/partial/unpaid).</summary>
    Task<List<InvoiceStatusBreakdownDto>> GetInvoiceStatusBreakdownAsync(int userId, HashSet<string> permissions);

    /// <summary>أحدث 5 فواتير.</summary>
    Task<List<RecentInvoiceDto>> GetRecentInvoicesAsync(int userId, HashSet<string> permissions, int count = 5);

    /// <summary>أحدث 5 أوامر إنتاج.</summary>
    Task<List<RecentProductionOrderDto>> GetRecentProductionOrdersAsync(int userId, HashSet<string> permissions, int count = 5);

    /// <summary>منتجات وصلت للحد الأدنى وتحتاج إعادة طلب.</summary>
    Task<List<LowStockItemDto>> GetLowStockItemsAsync(int userId, HashSet<string> permissions, int count = 5);

    /// <summary>أعلى 5 عملاء حسب إجمالي المشتريات.</summary>
    Task<List<TopCustomerDto>> GetTopCustomersAsync(int userId, HashSet<string> permissions, int count = 5);

    /// <summary>الأنشطة الأخيرة (audit log).</summary>
    Task<List<ActivityItemDto>> GetRecentActivityAsync(int userId, HashSet<string> permissions, int count = 8);
}

// ===== DTOs =====

public class DashboardKpisDto
{
    public decimal? TodaySales { get; set; }
    public int? TodayInvoicesCount { get; set; }
    public decimal? MonthSales { get; set; }
    public decimal? MonthSalesDeltaPct { get; set; }
    public int? ActiveCustomersCount { get; set; }
    public int? ActiveProductsCount { get; set; }
    public int? PendingProductionOrders { get; set; }
    public int? LowStockCount { get; set; }
    public decimal? OutstandingReceivables { get; set; }
    public int? PendingInvoices { get; set; }
}

public class SalesPointDto
{
    public DateTime Date { get; set; }
    public string DateLabel { get; set; } = string.Empty;
    public decimal Total { get; set; }
    public int InvoicesCount { get; set; }
}

public class InvoiceStatusBreakdownDto
{
    public string Status { get; set; } = string.Empty;
    public string StatusLabel { get; set; } = string.Empty;
    public int Count { get; set; }
    public decimal TotalAmount { get; set; }
}

public class RecentInvoiceDto
{
    public long Id { get; set; }
    public string InvoiceNumber { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public DateTime InvoiceDate { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal PaidAmount { get; set; }
    public string Status { get; set; } = string.Empty;
    public string Currency { get; set; } = "ج.م";
}

public class RecentProductionOrderDto
{
    public long Id { get; set; }
    public string ProductionNumber { get; set; } = string.Empty;
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public int QuantityCompleted { get; set; }
    public string Status { get; set; } = string.Empty;
    public string Priority { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public class LowStockItemDto
{
    public long ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string Sku { get; set; } = string.Empty;
    public decimal QuantityOnHand { get; set; }
    public int ReorderLevel { get; set; }
    public string WarehouseName { get; set; } = string.Empty;
}

public class TopCustomerDto
{
    public long Id { get; set; }
    public string DisplayName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public int TotalOrders { get; set; }
    public decimal TotalPurchases { get; set; }
    public DateTime? LastPurchaseDate { get; set; }
}

public class ActivityItemDto
{
    public int Id { get; set; }
    public string Action { get; set; } = string.Empty;
    public string ActorName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Icon { get; set; } = "info";
    public string Color { get; set; } = "primary";
}
